using FluentResults;
using Heyer.BuildingBlocks.Infrastructure.Messaging;
using Heyer.Modules.JobBoard.Domain.JobOffers;

namespace Heyer.Modules.JobBoard.Application.JobOffers.Create;

public class CreateJobOfferHandler : ICommandHandler<CreateJobOffer>
{
    private readonly IJobOffersRepository _jobOffersRepository;

    public CreateJobOfferHandler(IJobOffersRepository jobOffersRepository)
    {
        _jobOffersRepository = jobOffersRepository;
    }
    
    public async Task<Result> Handle(CreateJobOffer request, CancellationToken cancellationToken)
    {
        var jobOffer = JobOffer.CreateNew(
            request.CompanyDetails,
            request.OfferSummary,
            request.JobDescription,
            request.RemoteWork);

        var addResult = await _jobOffersRepository.AddAsync(jobOffer, cancellationToken);

        return addResult;
    }
}