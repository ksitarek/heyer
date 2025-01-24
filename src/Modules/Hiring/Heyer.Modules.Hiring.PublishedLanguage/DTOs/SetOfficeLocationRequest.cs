namespace Heyer.Modules.Hiring.PublishedLanguage.DTOs;

public record SetOfficeLocationRequest(
    Guid JobOfferId,
    string City,
    string Country);