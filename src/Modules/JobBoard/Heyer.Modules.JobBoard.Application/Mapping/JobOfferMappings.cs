using Heyer.Modules.Hiring.PublishedLanguage;
using Heyer.Modules.JobBoard.Domain.JobOffers;

namespace Heyer.Modules.JobBoard.Application.Mapping;

public static class JobOfferMappings
{
    public static PublishedJobOfferDetails MapToJobOfferDetails(this PublishedJobOffer publishedJobOffer) =>
        new(publishedJobOffer.Id.Guid,
            publishedJobOffer.CompanyDetails,
            publishedJobOffer.OfferSummary,
            publishedJobOffer.JobDescription,
            publishedJobOffer.Location!,
            publishedJobOffer.RemoteWork,
            publishedJobOffer.Requirements!,
            publishedJobOffer.ContractsDetails!.Select(x =>
                                                           new ContractDetails(
                                                               x.EmploymentType,
                                                               new SalaryRange(
                                                                   x.SalaryRange.IsPublished,
                                                                   x.SalaryRange.IsPublished ? x.SalaryRange.From : 0,
                                                                   x.SalaryRange.IsPublished ? x.SalaryRange.To : 0),
                                                               x.TimeNumerator,
                                                               x.TimeDenominator
                                                           )).ToList());
}