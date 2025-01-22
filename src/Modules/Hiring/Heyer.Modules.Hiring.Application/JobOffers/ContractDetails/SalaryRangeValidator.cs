using FluentValidation;
using Heyer.Modules.Hiring.PublishedLanguage.DTOs;

namespace Heyer.Modules.Hiring.Application.JobOffers.ContractDetails;

public class SalaryRangeValidator : AbstractValidator<SalaryRange>
{
    public SalaryRangeValidator()
    {
        RuleFor(x => x.From).NotNull().GreaterThan(0);

        RuleFor(x => x.To).NotNull()
            .GreaterThan(x => x.From);

        RuleFor(x => x.IsPublished).NotNull();
    }
}