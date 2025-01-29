using FluentValidation;

namespace Heyer.Modules.Hiring.Application.JobOffers.TakeDown;

public class TakeDownJobOfferValidator : AbstractValidator<TakeDownJobOffer>
{
    public TakeDownJobOfferValidator() => RuleFor(x => x.Id).NotNull().SetValidator(new JobOfferIdValidator());
}