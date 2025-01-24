using FluentResults;
using Heyer.BuildingBlocks.Application.Results;
using Heyer.BuildingBlocks.Infrastructure.Messaging;
using Heyer.Modules.Hiring.Domain.JobOffers;

namespace Heyer.Modules.Hiring.Application.JobOffers.Update;

public class UpdateJobOfferHandler : ICommandHandler<UpdateJobOffer>
{
    private readonly IJobOffersRepository _jobOffersRepository;

    public UpdateJobOfferHandler(IJobOffersRepository jobOffersRepository) =>
        _jobOffersRepository = jobOffersRepository;

    public async Task<Result> Handle(UpdateJobOffer request, CancellationToken cancellationToken)
    {
        var jobOffer = await _jobOffersRepository.GetJobOfferById(request.Id, cancellationToken);

        if (jobOffer is null)
        {
            return new NotFoundError();
        }

        return jobOffer.UpdateDescription(request.OfferSummary, request.JobDescription, request.RemoteWork);
    }
}