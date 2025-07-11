namespace BuildingBlocks.Domain.ValueObjects;

public class Address : ValueObject
{
    private Address(string country, string state, string city, string district, string street, string number, string zipCode, string complement)
    {
        Country = country;
        State = state;
        City = city;
        District = district;
        Street = street;
        Number = number;
        ZipCode = zipCode;
        Complement = complement;
    }

    private Address()
    {

    }

    public string Country { get; private set; }
    public string State { get; private set; }
    public string City { get; private set; }
    public string District { get; private set; }
    public string Street { get; private set; }
    public string Number { get; private set; }
    public string ZipCode { get; private set; }
    public string Complement { get; private set; }

    public static Address Create(string country, string state, string city, string district, string street, string number, string zipCode, string complement)
    {
        return new Address(country, state, city, district, street, number, zipCode, complement);
    }

    public static Address Empty()
    {
        return new Address("", "", "", "", "", "", "", "");
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Country;
        yield return State;
        yield return City;
        yield return District;
        yield return Street;
        yield return Number;
        yield return ZipCode;
        yield return Complement;
    }
}
