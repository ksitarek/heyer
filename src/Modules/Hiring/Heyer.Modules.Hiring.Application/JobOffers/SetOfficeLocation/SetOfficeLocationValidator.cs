using FluentValidation;

namespace Heyer.Modules.Hiring.Application.JobOffers.SetOfficeLocation;

public class SetOfficeLocationValidator : AbstractValidator<SetOfficeLocation>
{
    public SetOfficeLocationValidator()
    {
        RuleFor(x => x.Id).NotNull().SetValidator(new JobOfferIdValidator());
        RuleFor(x => x.Location).NotNull().DependentRules(() =>
        {
            RuleFor(x => x.Location.City).NotEmpty();
            RuleFor(x => x.Location.Country).NotEmpty();
        });
    }
}