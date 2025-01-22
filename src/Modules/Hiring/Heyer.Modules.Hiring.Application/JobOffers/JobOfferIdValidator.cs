using FluentValidation;
using Heyer.Modules.Hiring.Domain.JobOffers;

namespace Heyer.Modules.Hiring.Application.JobOffers;

public class JobOfferIdValidator : AbstractValidator<JobOfferId>
{
    public JobOfferIdValidator() => RuleFor(x => x.Guid).NotEmpty();
}