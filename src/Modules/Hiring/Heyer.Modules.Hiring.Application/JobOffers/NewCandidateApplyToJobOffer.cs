using Heyer.BuildingBlocks.Infrastructure.Messaging;
using Heyer.Modules.Hiring.Domain.JobOffers;

namespace Heyer.Modules.Hiring.Application.JobOffers;

public record NewCandidateApplyToJobOffer(
    JobOfferId JobOfferId,
    string FirstName,
    string LastName,
    string Email,
    string ResumeKey,
    bool IncludeInCandidatePool,
    Dictionary<string, object> Attributes) : ICommand;