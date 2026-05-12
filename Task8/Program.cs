using System.Globalization;

namespace Task8;

class Program
{
    public static void Main()
    {
        const string dateTimeIsoString = "2026-05-12T06:20:00.0000000Z";
        try
        {
            var dateTime = DateTimeOffset.ParseExact(dateTimeIsoString, "O", CultureInfo.InvariantCulture);
            Console.WriteLine("Date: " + dateTime);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}
