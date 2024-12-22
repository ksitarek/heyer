namespace Heyer.Modules.Hiring.Domain.JobOffers;

public class OfficeLocation
{
    public OfficeLocation(string city, string country)
    {
        City = city;
        Country = country;
    }

    private OfficeLocation()
    {
    }

    public string City { get; private set; } = null!;
    public string Country { get; private set; } = null!;
}