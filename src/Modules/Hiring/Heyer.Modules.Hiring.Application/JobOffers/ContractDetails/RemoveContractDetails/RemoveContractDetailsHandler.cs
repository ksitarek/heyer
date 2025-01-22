using FluentResults;
using Heyer.BuildingBlocks.Application.Results;
using Heyer.BuildingBlocks.Infrastructure.Messaging;
using Heyer.Modules.Hiring.Domain.JobOffers;

namespace Heyer.Modules.Hiring.Application.JobOffers.ContractDetails.RemoveContractDetails;

public class RemoveContractDetailsHandler : ICommandHandler<RemoveContractDetails>
{
    private readonly IJobOffersRepository _jobOffersRepository;

    public RemoveContractDetailsHandler(IJobOffersRepository jobOffersRepository) =>
        _jobOffersRepository = jobOffersRepository;

    public async Task<Result> Handle(RemoveContractDetails request, CancellationToken cancellationToken)
    {
        var jobOffer = await _jobOffersRepository.GetJobOfferById(request.Id, cancellationToken);

        if (jobOffer is null)
        {
            return new NotFoundError();
        }

        return jobOffer.RemoveContractDetails(request.EmploymentType);
    }
}