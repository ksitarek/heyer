using FluentResults;
using Heyer.BuildingBlocks.Application.Authorization;
using Heyer.BuildingBlocks.Infrastructure.Messaging;
using Heyer.Modules.JobBoard.Domain.Companies;
using Heyer.Modules.JobBoard.Domain.JobOffers;

namespace Heyer.Modules.JobBoard.Application.JobOffers.Create;

public class CreateJobOfferHandler : ICommandHandler<CreateJobOffer, Guid>
{
    private readonly IPublishedJobOffersRepository _publishedJobOffersRepository;
    private readonly IUserDataProvider _userDataProvider;

    public CreateJobOfferHandler(IUserDataProvider userDataProvider,
                                 IPublishedJobOffersRepository publishedJobOffersRepository)
    {
        _userDataProvider = userDataProvider;
        _publishedJobOffersRepository = publishedJobOffersRepository;
    }

    public async Task<Result<Guid>> Handle(CreateJobOffer request, CancellationToken cancellationToken)
    {
        var companyDetails = new CompanyDetails(
            new CompanyId(_userDataProvider.CompanyId),
            _userDataProvider.CompanyName);

        var jobOffer = PublishedJobOffer.CreateNew(
            companyDetails,
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