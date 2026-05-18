using System.Collections.Immutable;

namespace Task11;

public record PaginatedList<T>
{
    public ImmutableArray<T> List { get; }
    public ImmutableArray<int> PageSizes { get; }

    public int Page { get; private init; }
    public int PageSize { get; private init;  }

    public ImmutableArray<T> PageList { get; private init;  }
    public int LastPage { get; private init; }

    private const int FirstPage = 1;

    private PaginatedList
    (
        ImmutableArray<T> list,
        ImmutableArray<int> pageSizes,
        int page,
        int pageSize,
        ImmutableArray<T> pageList,
        int lastPage
    )
    {
        List = list;
        PageSizes = pageSizes;
        Page = page;
        PageSize = pageSize;
        PageList = pageList;
        LastPage = lastPage;
    }

    public static PaginatedList<T> Create(ImmutableArray<T> list, ImmutableArray<int> pageSizes)
    {
        if (pageSizes.Length == 0)
        {
            throw new ArgumentException("Список с количеством записей на странице должен быть непустым.");
        }
        if (pageSizes.Any(s => s <= 0))
        {
            throw new ArgumentException("Количество записей на странице должно быть больше 0.");
        }

        var pageSize = pageSizes.First();
        return new PaginatedList<T>
        (
            list: list,
            pageSizes: pageSizes,
            page: FirstPage,
            pageSize: pageSize,
            pageList: GetPageList(list, FirstPage, pageSize),
            lastPage: GetLastPage(list, pageSize)
        );
    }

    public static PaginatedList<T> SetPage(PaginatedList<T> paginatedList, int page)
    {
        var clampedPage = Math.Clamp(page, FirstPage, paginatedList.LastPage);
        return paginatedList with
        {
            Page = clampedPage,
            PageList = GetPageList(paginatedList.List, clampedPage, paginatedList.PageSize)
        };
    }

    public static PaginatedList<T> SetPageSize(PaginatedList<T> paginatedList, int pageSize)
    {
        if (paginatedList.PageSizes.All(s => s != pageSize))
        {
            throw new Exception("Количество записей на странице должно соответствовать одному из списка.");
        }

        var lastPage = GetLastPage(paginatedList.List, pageSize);
        var clampedPage = Math.Min(paginatedList.Page, lastPage);

        return paginatedList with
        {
            PageSize = pageSize,
            Page = clampedPage,
            PageList = GetPageList(paginatedList.List, clampedPage, pageSize),
            LastPage = lastPage
        };
    }

    private static ImmutableArray<T> GetPageList(ImmutableArray<T> list, int page, int pageSize)
    {
        return list.Skip((page - 1) * pageSize).Take(pageSize).ToImmutableArray();
    }

    private static int GetLastPage(ImmutableArray<T> list, int pageSize)
    {
        return Math.Max
        (
            FirstPage,
            (list.Length + pageSize - 1) / pageSize
        );
    }

    public override string ToString()
    {
        return $"Список: [{string.Join(", ", List)}]\n" +
               $"Список с количествами записей на странице: {string.Join(", ", PageSizes)}\n" +
               $"Страница: {Page} из {LastPage}, " +
               $"Количество записей на странице: {PageSize}\n" +
               $"Список записей на странице: [{string.Join(", ", PageList)}]";
    }
}

class Program
{
    public static void Main()
    {
        var paginatedList = PaginatedList<int>.Create
        (
            Enumerable.Range(0, 25).ToImmutableArray(),
            [1, 5, 10, 25]
        );
        Console.WriteLine(paginatedList);

        paginatedList = PaginatedList<int>.SetPageSize(paginatedList, 10);
        Console.WriteLine("\n" + paginatedList);

        paginatedList = PaginatedList<int>.SetPage(paginatedList, 4);
        Console.WriteLine("\n" + paginatedList);

        paginatedList = PaginatedList<int>.SetPageSize(paginatedList, 25);
        Console.WriteLine("\n" + paginatedList);

        paginatedList = PaginatedList<int>.SetPage(paginatedList, -1);
        Console.WriteLine("\n" + paginatedList);

        var emptyPaginatedList = PaginatedList<int>.Create([], [5, 10]);
        Console.WriteLine("\n" + emptyPaginatedList);

        emptyPaginatedList = PaginatedList<int>.SetPageSize(emptyPaginatedList, 10);
        Console.WriteLine("\n" + emptyPaginatedList);

        emptyPaginatedList = PaginatedList<int>.SetPage(emptyPaginatedList, 2);
        Console.WriteLine("\n" + emptyPaginatedList);
    }
}
