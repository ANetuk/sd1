using System.Collections.Immutable;

namespace Task12;

public interface IPageSizeOption<T>
{
    public string Name { get; }
    public Func<ImmutableArray<T>, int> GetPageSize { get; }
}

public record NumericPageSizeOption<T> : IPageSizeOption<T>
{
    private readonly int _pageSize;

    public string Name => _pageSize.ToString();
    public Func<ImmutableArray<T>, int> GetPageSize => _ => _pageSize;

    public NumericPageSizeOption(int pageSize)
    {
        if (pageSize <= 0)
        {
            throw new ArgumentException("Количество записей на странице должно быть больше 0.");
        }
        _pageSize = pageSize;
    }
}

public record AllPageSizeOption<T> : IPageSizeOption<T>
{
    public string Name => "Все";
    public Func<ImmutableArray<T>, int> GetPageSize { get; } = items => items.Length;
}

public record PaginatedItems<T>
{
    public ImmutableArray<T> Items { get; }
    public ImmutableArray<IPageSizeOption<T>> PageSizeOptions { get; }

    public int Page { get; private init; }
    public IPageSizeOption<T> PageSizeOption { get; private init;  }

    public ImmutableArray<T> PageItems { get; private init;  }
    public int LastPage { get; private init; }

    private const int FirstPage = 1;

    private PaginatedItems
    (
        ImmutableArray<T> items,
        ImmutableArray<IPageSizeOption<T>> pageSizeOptions,
        int page,
        IPageSizeOption<T> pageSizeOption,
        ImmutableArray<T> pageItems,
        int lastPage
    )
    {
        Items = items;
        PageSizeOptions = pageSizeOptions;
        Page = page;
        PageSizeOption = pageSizeOption;
        PageItems = pageItems;
        LastPage = lastPage;
    }

    public static PaginatedItems<T> Create(ImmutableArray<T> items, ImmutableArray<IPageSizeOption<T>> pageSizeOptions)
    {
        if (pageSizeOptions.Length == 0)
        {
            throw new ArgumentException("Список с количеством записей на странице должен быть непустым.");
        }

        var pageSizeOption = pageSizeOptions.First();
        return new PaginatedItems<T>
        (
            items: items,
            pageSizeOptions: pageSizeOptions,
            page: FirstPage,
            pageSizeOption: pageSizeOption,
            pageItems: GetPageItems(items, FirstPage, pageSizeOption),
            lastPage: GetLastPage(items, pageSizeOption)
        );
    }

    public static PaginatedItems<T> SetPage(PaginatedItems<T> paginatedItems, int page)
    {
        var clampedPage = Math.Clamp(page, FirstPage, paginatedItems.LastPage);
        return paginatedItems with
        {
            Page = clampedPage,
            PageItems = GetPageItems(paginatedItems.Items, clampedPage, paginatedItems.PageSizeOption)
        };
    }

    public static PaginatedItems<T> SetPageSizeOption
    (
        PaginatedItems<T> paginatedItems, IPageSizeOption<T> pageSizeOption
    )
    {
        if (paginatedItems.PageSizeOptions.All(s => !s.Equals(pageSizeOption)))
        {
            throw new Exception("Количество записей на странице должно соответствовать одному из списка.");
        }

        var lastPage = GetLastPage(paginatedItems.Items, pageSizeOption);
        var clampedPage = Math.Min(paginatedItems.Page, lastPage);

        return paginatedItems with
        {
            PageSizeOption = pageSizeOption,
            Page = clampedPage,
            PageItems = GetPageItems(paginatedItems.Items, clampedPage, pageSizeOption),
            LastPage = lastPage
        };
    }

    private static ImmutableArray<T> GetPageItems(ImmutableArray<T> items, int page, IPageSizeOption<T> pageSizeOption)
    {
        var pageSize = pageSizeOption.GetPageSize(items);
        return items.Skip((page - 1) * pageSize).Take(pageSize).ToImmutableArray();
    }

    private static int GetLastPage(ImmutableArray<T> list, IPageSizeOption<T> pageSizeOption)
    {
        var pageSize = pageSizeOption.GetPageSize(list);
        return Math.Max
        (
            FirstPage,
            (list.Length + pageSize - 1) / pageSize
        );
    }

    public override string ToString()
    {
        var pageSizeOptionNames = PageSizeOptions.Select(o => o.Name);
        return $"Список: [{string.Join(", ", Items)}]\n" +
               $"Список с количествами записей на странице: {string.Join(", ", pageSizeOptionNames)}\n" +
               $"Страница: {Page} из {LastPage}, " +
               $"Количество записей на странице: {PageSizeOption.Name}\n" +
               $"Список записей на странице: [{string.Join(", ", PageItems)}]";
    }
}

class Program
{
    public static void Main()
    {
        var paginatedNumbers = PaginatedItems<int>.Create
        (
            Enumerable.Range(0, 25).ToImmutableArray(),
            [
                new NumericPageSizeOption<int>(1),
                new NumericPageSizeOption<int>(5),
                new NumericPageSizeOption<int>(10),
                new NumericPageSizeOption<int>(30),
                new AllPageSizeOption<int>()
            ]
        );
        Console.WriteLine(paginatedNumbers);

        paginatedNumbers = PaginatedItems<int>.SetPageSizeOption
        (
            paginatedNumbers, new NumericPageSizeOption<int>(10)
        );
        Console.WriteLine("\n" + paginatedNumbers);

        paginatedNumbers = PaginatedItems<int>.SetPage(paginatedNumbers, 4);
        Console.WriteLine("\n" + paginatedNumbers);

        paginatedNumbers = PaginatedItems<int>.SetPage(paginatedNumbers, -1);
        Console.WriteLine("\n" + paginatedNumbers);

        paginatedNumbers = PaginatedItems<int>.SetPageSizeOption
        (
            paginatedNumbers, new AllPageSizeOption<int>()
        );
        Console.WriteLine("\n" + paginatedNumbers);
    }
}
