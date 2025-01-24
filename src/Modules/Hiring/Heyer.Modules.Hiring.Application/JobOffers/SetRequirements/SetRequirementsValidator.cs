using FluentValidation;

namespace Heyer.Modules.Hiring.Application.JobOffers.SetRequirements;

public class SetRequirementsValidator : AbstractValidator<SetRequirements>
{
    public SetRequirementsValidator()
    {
        RuleFor(x => x.Id).NotEmpty().SetValidator(new JobOfferIdValidator());
        RuleFor(x => x.Requirements).NotNull().DependentRules(() =>
        {
            RuleFor(x => x.Requirements.ExperienceLevel).IsInEnum();
            RuleForEach(x => x.Requirements.Skills).SetValidator(new SkillValidator());
        });
    }
}