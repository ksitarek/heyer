using Heyer.BuildingBlocks.Infrastructure.Messaging;
using Heyer.Modules.JobBoard.Domain.JobOffers;

namespace Heyer.Modules.JobBoard.Application.JobOffers.NewCandidateApply;

public record NewCandidateApplyToJobOffer(
    PublishedJobOfferId PublishedJobOfferId,
    string FirstName,
    string LastName,
    string Email,
    string ResumeKey,
    bool IncludeInCandidatePool,
    Dictionary<string, object> Attributes) : ICommand;