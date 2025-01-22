using FluentResults;
using Heyer.BuildingBlocks.Application.Results;
using Heyer.BuildingBlocks.Infrastructure.Messaging;
using Heyer.Modules.Hiring.Domain.JobOffers;

namespace Heyer.Modules.Hiring.Application.JobOffers.ContractDetails.UpdateContractDetails;

public class UpdateContractDetailsHandler : ICommandHandler<UpdateContractDetails>
{
    private readonly IJobOffersRepository _jobOffersRepository;

    public UpdateContractDetailsHandler(IJobOffersRepository jobOffersRepository) =>
        _jobOffersRepository = jobOffersRepository;

    public async Task<Result> Handle(UpdateContractDetails request, CancellationToken cancellationToken)
    {
        var jobOffer = await _jobOffersRepository.GetJobOfferById(request.Id, cancellationToken);

        if (jobOffer == null)
        {
            return new NotFoundError();
        }

        var removeContractDetailsResult = jobOffer.RemoveContractDetails(request.EmploymentType);

        if (removeContractDetailsResult.IsFailed)
        {
            return removeContractDetailsResult;
        }

        var newContractDetails = new PublishedLanguage.DTOs.ContractDetails(
            request.EmploymentType,
            request.SalaryRange,
            request.TimeNumerator,
            request.TimeDenominator);

        var addContractDetailsResult = jobOffer.AddContractDetails(newContractDetails);

        return addContractDetailsResult;
    }
}