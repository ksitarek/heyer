using FluentValidation;

namespace Heyer.Modules.Hiring.Application.JobOffers.ContractDetails.UpdateContractDetails;

public class UpdateContractDetailsValidator : AbstractValidator<UpdateContractDetails>
{
    public UpdateContractDetailsValidator()
    {
        RuleFor(x => x.Id).NotEmpty().SetValidator(new JobOfferIdValidator());

        RuleFor(x => x.EmploymentType).IsInEnum();
        RuleFor(x => x.SalaryRange).NotNull().SetValidator(new SalaryRangeValidator());
    }
}