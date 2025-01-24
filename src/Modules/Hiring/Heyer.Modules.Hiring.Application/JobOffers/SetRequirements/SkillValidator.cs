using FluentValidation;
using Heyer.Modules.Hiring.PublishedLanguage.DTOs;

namespace Heyer.Modules.Hiring.Application.JobOffers.SetRequirements;

public class SkillValidator : AbstractValidator<Skill>
{
    public SkillValidator()
    {
        RuleFor(x => x.Label).NotEmpty();
        RuleFor(x => x.Level).IsInEnum();
    }
}