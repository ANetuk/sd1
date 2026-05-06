namespace Task5;

public enum Grade : sbyte
{
    Terrible = 1,
    Bad = 2,
    Average = 3,
    Good = 4,
    Perfect = 5
}

public static class GradeCalculator
{
    public static double CalculateAverage(Grade[] grades)
    {
        if (grades.Length == 0) return double.NaN;

        var sum = grades.Aggregate(0, (acc, grade) => acc + (int)grade);
        var average = (double)sum / grades.Length;
        var roundedAverage = Math.Round(average, 2, MidpointRounding.AwayFromZero);
        return roundedAverage;
    }
}
