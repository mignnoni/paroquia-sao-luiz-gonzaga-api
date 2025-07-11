namespace BuildingBlocks.Domain.ValueObjects;

public class DateRange : ValueObject
{
    private DateRange(int month, int year)
    {
        Month = month;
        Year = year;
    }

    private DateRange()
    {

    }

    public int Month { get; private set; }
    public int Year { get; private set; }

    public static DateRange Create(int month, int year)
    {
        return new DateRange(month, year);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Month;
        yield return Year;
    }
}
