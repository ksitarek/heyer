using System.Text.Json.Serialization;

namespace Heyer.Modules.Hiring.PublishedLanguage;

public class OfficeLocation
{
    [JsonConstructor]
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