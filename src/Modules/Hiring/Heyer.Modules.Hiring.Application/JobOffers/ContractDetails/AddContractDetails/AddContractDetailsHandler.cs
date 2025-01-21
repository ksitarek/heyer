using FluentResults;
using FluentValidation;
using Heyer.BuildingBlocks.Application.Results;
using Heyer.BuildingBlocks.Infrastructure.Messaging;
using Heyer.Modules.Hiring.Domain.JobOffers;

namespace Heyer.Modules.Hiring.Application.JobOffers.ContractDetails.AddContractDetails;

public class AddContractDetailsHandler : ICommandHandler<AddContractDetails>
{
    private readonly IJobOffersRepository _jobOffersRepository;

    public AddContractDetailsHandler(IJobOffersRepository jobOffersRepository) =>
        _jobOffersRepository = jobOffersRepository;

    public async Task<Result> Handle(AddContractDetails request, CancellationToken cancellationToken)
    {
        var jobOffer = await _jobOffersRepository.GetJobOfferById(request.Id, cancellationToken);

        if (jobOffer is null)
        {
            return new NotFoundError();
        }

        return jobOffer.AddContractDetails(request.ContractDetails);
    }
}

public class AddContractDetailsValidator : AbstractValidator<AddContractDetails>
{
    public AddContractDetailsValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ContractDetails).NotNull()
            .DependentRules(() =>
            {
                RuleFor(x => x.ContractDetails.SalaryRange).NotNull()
                    .DependentRules(() =>
                    {
                        RuleFor(x => x.ContractDetails.SalaryRange.From).NotNull().GreaterThan(0);

                        RuleFor(x => x.ContractDetails.SalaryRange.To).NotNull()
                            .GreaterThan(x => x.ContractDetails.SalaryRange.From);

                        RuleFor(x => x.ContractDetails.SalaryRange.IsPublished).NotNull();
                    });
            });
    }
}