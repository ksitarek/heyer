using FluentResults;
using Heyer.BuildingBlocks.Application.Results;
using Heyer.BuildingBlocks.Infrastructure.Messaging;
using Heyer.Modules.Hiring.Domain.JobOffers;

namespace Heyer.Modules.Hiring.Application.JobOffers.TakeDown;

public class TakeDownJobOfferHandler : ICommandHandler<TakeDownJobOffer>
{
    private readonly IJobOffersRepository _jobOffersRepository;

    public TakeDownJobOfferHandler(IJobOffersRepository jobOffersRepository) =>
        _jobOffersRepository = jobOffersRepository;

    public async Task<Result> Handle(TakeDownJobOffer request, CancellationToken cancellationToken)
    {
        var jobOffer = await _jobOffersRepository.GetJobOfferById(request.Id, cancellationToken);

        if (jobOffer is null)
        {
            return new NotFoundError();
        }

        return jobOffer.TakeDown();
    }
}