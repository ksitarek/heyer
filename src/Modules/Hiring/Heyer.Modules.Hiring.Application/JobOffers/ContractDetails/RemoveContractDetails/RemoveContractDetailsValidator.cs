using FluentValidation;

namespace Heyer.Modules.Hiring.Application.JobOffers.ContractDetails.RemoveContractDetails;

public class RemoveContractDetailsValidator : AbstractValidator<RemoveContractDetails>
{
    public RemoveContractDetailsValidator()
    {
        RuleFor(x => x.Id).NotEmpty().SetValidator(new JobOfferIdValidator());

        RuleFor(x => x.EmploymentType).IsInEnum();
    }
}