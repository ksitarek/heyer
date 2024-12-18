namespace Heyer.Modules.JobBoard.Domain.JobOffers;

public record OfficeLocation
{
    public string City { get; }
    public string Country { get; }
    
    private OfficeLocation()
    {
        
    }

    public OfficeLocation(string city, string country)
    {
        City = city;
        Country = country;
    }
};