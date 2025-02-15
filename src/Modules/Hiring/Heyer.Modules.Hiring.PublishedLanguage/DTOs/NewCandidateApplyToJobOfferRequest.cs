namespace Heyer.Modules.Hiring.PublishedLanguage.DTOs;

public record NewCandidateApplyToJobOfferRequest(
    Guid JobOfferId,
    string FirstName,
    string LastName,
    string Email,
    string ResumeKey,
    bool IncludeInCandidatePool,
    Dictionary<string, object> Attributes);