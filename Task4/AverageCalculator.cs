namespace Task4;

public static class AverageCalculator
{
    public static float CalculateAverage(int[] numbers)
    {
        return (float)numbers.Sum() / numbers.Length;
    }
}
