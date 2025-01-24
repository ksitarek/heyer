namespace Heyer.Modules.Hiring.PublishedLanguage.DTOs;

public record SetRequirementsRequest(
    Guid JobOfferId,
    Requirements Requirements);