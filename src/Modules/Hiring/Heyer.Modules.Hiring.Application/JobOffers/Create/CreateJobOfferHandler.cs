using FluentResults;
using Heyer.BuildingBlocks.Application.Authorization;
using Heyer.BuildingBlocks.Infrastructure.Messaging;
using Heyer.Modules.Hiring.Domain.JobOffers;

namespace Heyer.Modules.Hiring.Application.JobOffers.Create;

public class CreateJobOfferHandler : ICommandHandler<CreateJobOffer, Guid>
{
    private readonly IJobOffersRepository _publishedJobOffersRepository;
    private readonly IUserDataProvider _userDataProvider;

    public CreateJobOfferHandler(IUserDataProvider userDataProvider,
                                 IJobOffersRepository publishedJobOffersRepository)
    {
        _userDataProvider = userDataProvider;
        _publishedJobOffersRepository = publishedJobOffersRepository;
    }

    public async Task<Result<Guid>> Handle(CreateJobOffer request, CancellationToken cancellationToken)
    {
        var jobOffer = JobOffer.CreateNew(
            JobOfferId.CreateNew(),
            request.OfferSummary,
            request.JobDescription,
            request.RemoteWork);

        var addResult = await _publishedJobOffersRepository.AddAsync(jobOffer, cancellationToken);

        if (addResult.IsSuccess)
        {
            return jobOffer.Id.Guid;
        }

        return addResult;
    }
}