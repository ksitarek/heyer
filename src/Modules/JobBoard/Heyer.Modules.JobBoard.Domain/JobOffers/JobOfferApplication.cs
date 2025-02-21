namespace Heyer.Modules.JobBoard.Domain.JobOffers;

public record JobOfferApplication(
    string FirstName,
    string LastName,
    string Email,
    string ResumeKey,
    bool IncludeInCandidatePool,
    Dictionary<string, object> Attributes);