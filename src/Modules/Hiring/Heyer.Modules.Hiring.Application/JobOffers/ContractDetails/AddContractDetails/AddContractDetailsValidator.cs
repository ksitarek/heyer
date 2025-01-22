using FluentValidation;

namespace Heyer.Modules.Hiring.Application.JobOffers.ContractDetails.AddContractDetails;

public class AddContractDetailsValidator : AbstractValidator<AddContractDetails>
{
    public AddContractDetailsValidator()
    {
        RuleFor(x => x.Id).NotEmpty().SetValidator(new JobOfferIdValidator());
        RuleFor(x => x.ContractDetails).NotNull()
            .DependentRules(() =>
            {
                RuleFor(x => x.ContractDetails.EmploymentType).IsInEnum();

                RuleFor(x => x.ContractDetails.SalaryRange).NotNull()
                    .SetValidator(new SalaryRangeValidator());
            });
    }
}