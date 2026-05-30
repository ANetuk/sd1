namespace Task18;

public interface IFieldValidator<TDocument>
{
    public string FieldName { get; }
    public string? Validate(TDocument document);
}

public class FieldErrorsList
{
    public required string FieldName { get; init; }
    public required IEnumerable<string> Errors { get; init; }
}

public static class DocumentValidator<TDocument>
{
    public static string Validate(
        IEnumerable<IFieldValidator<TDocument>> validators,
        TDocument document
    ) {
        return string.Join(
            "\n",
            validators
                .GroupBy(v => v.FieldName)
                .Select(v => new FieldErrorsList
                {
                    FieldName = v.Key,
                    Errors = v.Select(fv => fv.Validate(document)).Where(e => e is string && e != "")
                })
                .Where(l => l.Errors.Any())
                .Select(l => $"{l.FieldName}: {string.Join(", ", l.Errors)}")
        );
    }
}

public enum Currency
{
    RUB
}

public class Check
{
    public int Number { get; set; }
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public Currency Currency { get; set; } 
}

public class NumberErrorValidator : IFieldValidator<Check>
{
    public string FieldName => "Номер счёта";

    public string? Validate(Check document)
    {
        if (document.Number <= 0)
        {
            return "Номер счёта должен быть больше нуля";
        }
        return null;
    }
}

public class DateErrorValidator : IFieldValidator<Check>
{
    public string FieldName => "Дата чека";
    public string? Validate(Check document)
    {
        if (document.Date == null)
        {
            return "Дата чека обязательна для заполнения";
        }

        if (document.Date.Date > DateTime.Now.Date)
        {
            return "Дата чека не может быть позже сегодняшнего дня";
        }

        return null;
    }
}

public class AmountErrorValidator : IFieldValidator<Check>
{
    public string FieldName => "Сумма чека";
    public string? Validate(Check document)
    {
        if (document.Amount <= 0)
        {
            return "Сумма чека должна быть больше нуля";
        }

        return null;
    }
}

public class DateWarningValidator : IFieldValidator<Check>
{
    public string FieldName => "Дата чека";
    public string? Validate(Check document)
    {
        if (document.Date.Date < DateTime.Now.Date)
        {
            return "Дата чека указана раньше сегодняшней";
        }

        return null;
    }
}

class Program
{
    static void Main()
    {
        var incorrectCheck = new Check
        {
            Number = -1,
            Date = DateTime.Now.AddDays(-1),
            Amount = -1,
            Currency = Currency.RUB
        };

        var checkErrorValidators = new IFieldValidator<Check>[]
        {
            new NumberErrorValidator(), new DateErrorValidator(), new AmountErrorValidator()
        };

        var errorMessage = DocumentValidator<Check>.Validate(checkErrorValidators, incorrectCheck);
        
        Console.WriteLine($"Ошибки:\n{errorMessage}");

        var checkWarningValidators = new IFieldValidator<Check>[]
        {
            new DateWarningValidator()
        };

        var warningMessage = DocumentValidator<Check>.Validate(checkWarningValidators, incorrectCheck);
        
        Console.WriteLine($"Предупреждения:\n{warningMessage}");
    }
}