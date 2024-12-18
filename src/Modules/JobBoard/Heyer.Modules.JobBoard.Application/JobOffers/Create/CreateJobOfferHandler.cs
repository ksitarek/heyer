using FluentResults;
using Heyer.BuildingBlocks.Application.Authorization;
using Heyer.BuildingBlocks.Infrastructure.Messaging;
using Heyer.Modules.JobBoard.Domain.Companies;
using Heyer.Modules.JobBoard.Domain.JobOffers;

namespace Heyer.Modules.JobBoard.Application.JobOffers.Create;

public class CreateJobOfferHandler : ICommandHandler<CreateJobOffer>
{
    private readonly IUserDataProvider _userDataProvider;
    private readonly IJobOffersRepository _jobOffersRepository;

    public CreateJobOfferHandler(IUserDataProvider userDataProvider, IJobOffersRepository jobOffersRepository)
    {
        _userDataProvider = userDataProvider;
        _jobOffersRepository = jobOffersRepository;
    }
    
    public async Task<Result> Handle(CreateJobOffer request, CancellationToken cancellationToken)
    {
        var companyDetails = new CompanyDetails(
            new CompanyId(_userDataProvider.CompanyId),
            _userDataProvider.CompanyName);
        
        var jobOffer = JobOffer.CreateNew(
            companyDetails,
            request.OfferSummary,
            request.JobDescription,
            request.RemoteWork);

        var addResult = await _jobOffersRepository.AddAsync(jobOffer, cancellationToken);

        return addResult;
    }
}