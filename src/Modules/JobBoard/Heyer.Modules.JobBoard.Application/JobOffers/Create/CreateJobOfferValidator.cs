using FluentValidation;
using Heyer.Modules.JobBoard.Domain.JobOffers;

namespace Heyer.Modules.JobBoard.Application.JobOffers.Create;

public class CreateJobOfferValidator : AbstractValidator<CreateJobOffer>
{
    public CreateJobOfferValidator()
    {
        RuleFor(x => x.OfferSummary)
            .NotEmpty()
            .MaximumLength(100)
            .MinimumLength(10);

        RuleFor(x => x.JobDescription)
            .NotEmpty()
            .MinimumLength(100);

        RuleFor(x => x.RemoteWork)
            .NotNull()
            .Must((_, remoteWork) => remoteWork != RemoteWork.Unknown).WithMessage("Remote work must be specified.");
    }
}