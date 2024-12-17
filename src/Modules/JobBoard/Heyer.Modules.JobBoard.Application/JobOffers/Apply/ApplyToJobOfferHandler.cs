using FluentResults;
using Heyer.BuildingBlocks.Infrastructure.Messaging;

namespace Heyer.Modules.JobBoard.Application.JobOffers.Apply;

public class ApplyToJobOfferHandler : ICommandHandler<ApplyToJobOffer>
{
    public Task<Result> Handle(ApplyToJobOffer request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}