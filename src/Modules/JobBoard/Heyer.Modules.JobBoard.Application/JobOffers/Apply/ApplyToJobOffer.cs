using Heyer.BuildingBlocks.Infrastructure.Messaging;
using Heyer.Modules.JobBoard.Domain.JobOffers;

namespace Heyer.Modules.JobBoard.Application.JobOffers.Apply;

public record ApplyToJobOffer(
    JobOfferId JobOfferId,
    string FirstName,
    string LastName,
    string Email,
    string ResumeKey,
    bool IncludeInCandidatePool,
    Dictionary<string, object> Attributes) : ICommand;