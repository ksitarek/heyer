namespace Heyer.Modules.Hiring.PublishedLanguage.DTOs;

public record NewCandidateApplyToJobOfferRequest(
    Guid PublishedJobOfferId,
    string FirstName,
    string LastName,
    string Email,
    string ResumeKey,
    bool IncludeInCandidatePool,
    Dictionary<string, object> Attributes);