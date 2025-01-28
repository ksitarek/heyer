using FluentValidation;

namespace Heyer.Modules.Hiring.Application.JobOffers.CheckForConflicts;

public class CheckForConflictsValidator : AbstractValidator<CheckForConflicts>
{
    public CheckForConflictsValidator() => RuleFor(x => x.Id).NotNull().SetValidator(new JobOfferIdValidator());
}