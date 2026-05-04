namespace Task4;

public class AverageCalculatorTests
{
    [Test]
    public void EmptyArrayHasNaNAverage()
    {
        var average = AverageCalculator.CalculateAverage([]);
        const float expected = float.NaN;
        Assert.That(average, Is.EqualTo(expected));
    }

    [Test]
    public void OnlyOneIntHasSelfAverage()
    {
        const int integer = 100;
        var average = AverageCalculator.CalculateAverage([integer]);
        const float expected = integer;
        Assert.That(average, Is.EqualTo(expected));
    }

    [Test]
    public void NegativeIntsHaveNegativeAverage()
    {
        var average = AverageCalculator.CalculateAverage([-1, -2]);
        const float expected = -1.5f;
        Assert.That(average, Is.EqualTo(expected));
    }

    [Test]
    public void PositiveIntsHavePositiveAverage()
    {
        var average = AverageCalculator.CalculateAverage([2, 4, 6]);
        const float expected = 4;
        Assert.That(average, Is.EqualTo(expected));
    }

    [Test]
    public void DifferentSignsButSameIntsHaveZeroAverage()
    {
        const int integer = 1000000;
        var average = AverageCalculator.CalculateAverage([integer, -integer]);
        const float expected = 0;
        Assert.That(average, Is.EqualTo(expected));
    }

    [Test]
    public void SameIntsArrayHasSameAverage()
    {
        const int integer = 1000000;
        const int length = 500;
        var average = AverageCalculator.CalculateAverage
        (
            Enumerable.Repeat(integer, length).ToArray()
        );
        const float expected = integer;
        Assert.That(average, Is.EqualTo(expected));
    }

    /* Кажется, метод работает правильно...
     * Однако при работе с большими числами возникают ошибки потери точности,
     * связанные с выбором float в качестве возвращаемого типа данных.
     * Следующий тест мог бы выявить проблему...
    [Test]
    public void BigSameIntsHaveSameAverage()
    {
        var average = AverageCalculator.CalculateAverage(
            [16777217, 16777217, 16777217]
        );
        const float expected = 16777217;
        Assert.That(average, Is.EqualTo(expected));
    }
    */
}
