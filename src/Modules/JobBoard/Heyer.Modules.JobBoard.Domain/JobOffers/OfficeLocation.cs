namespace Heyer.Modules.JobBoard.Domain.JobOffers;

public record OfficeLocation
{
    public string City { get; private set; } = null!;
    public string Country { get; private set; } = null!;
    
    private OfficeLocation()
    {
        
    }

    public OfficeLocation(string city, string country)
    {
        City = city;
        Country = country;
    }
};