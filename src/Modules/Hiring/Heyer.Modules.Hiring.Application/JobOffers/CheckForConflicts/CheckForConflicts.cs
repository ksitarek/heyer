using Heyer.BuildingBlocks.Infrastructure.Messaging;
using Heyer.Modules.Hiring.Domain.JobOffers;

namespace Heyer.Modules.Hiring.Application.JobOffers.CheckForConflicts;

public record CheckForConflicts(JobOfferId Id) : IQuery<bool>;