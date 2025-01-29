using Heyer.BuildingBlocks.Infrastructure.Messaging;
using Heyer.Modules.Hiring.Domain.JobOffers;

namespace Heyer.Modules.Hiring.Application.JobOffers.TakeDown;

public record TakeDownJobOffer(JobOfferId Id) : ICommand;