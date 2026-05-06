namespace Task5;

public class Tests
{
    [Test]
    public void ForEmptyListItReturnsNaN()
    {
        var averageGrade = GradeCalculator.CalculateAverage([]);
        Assert.That(averageGrade, Is.EqualTo(double.NaN));
    }

    [Test]
    public void ForListWithOneGradeItReturnsSameGrade()
    {
        var averageGrade = GradeCalculator.CalculateAverage([Grade.Perfect]);
        Assert.That(averageGrade, Is.EqualTo(5));
    }

    [Test]
    public void ForSomeGradesItReturnsAverage()
    {
        var averageGrade = GradeCalculator.CalculateAverage([Grade.Perfect, Grade.Terrible]);
        Assert.That(averageGrade, Is.EqualTo(3));
    }

    [Test]
    public void ItCanReturnFractionAverage()
    {
        var averageGrade = GradeCalculator.CalculateAverage([Grade.Terrible, Grade.Bad]);
        Assert.That(averageGrade, Is.EqualTo(1.5));
    }

    [Test]
    public void ItRoundsAverageToTwoDigits()
    {
        var averageGrade = GradeCalculator.CalculateAverage([
            Grade.Terrible, Grade.Terrible, Grade.Bad
        ]);
        Assert.That(averageGrade, Is.EqualTo(1.33));
    }

    [Test]
    public void ItRoundAverageToTheNearest()
    {
        var averageGrade = GradeCalculator.CalculateAverage([
            Grade.Terrible, Grade.Bad, Grade.Bad
        ]);
        Assert.That(averageGrade, Is.EqualTo(1.67));
    }
}
