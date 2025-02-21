using Heyer.Modules.Hiring.PublishedLanguage.DTOs;
using Heyer.Modules.JobBoard.Application.JobOffers.NewCandidateApply;
using Heyer.Modules.JobBoard.Domain.JobOffers;
using Heyer.Modules.JobBoard.PublishedLanguage.DTOs;

namespace Heyer.Modules.JobBoard.Application.Mapping;

public static class JobOfferMappings
{
    public static NewCandidateApplyToJobOffer MapToCommand(this NewCandidateApplyToJobOfferRequest request) => new(
        new PublishedJobOfferId(request.PublishedJobOfferId),
        request.FirstName,
        request.LastName,
        request.Email,
        request.ResumeKey,
        request.IncludeInCandidatePool,
        request.Attributes);

    public static PublishedJobOfferDetails MapToJobOfferDetails(this PublishedJobOffer publishedJobOffer) =>
        new(publishedJobOffer.Id.Guid,
            publishedJobOffer.CompanyDetails,
            publishedJobOffer.OfferSummary,
            publishedJobOffer.JobDescription,
            publishedJobOffer.Location,
            publishedJobOffer.RemoteWork,
            publishedJobOffer.Requirements,
            publishedJobOffer.ContractsDetails.Select(x =>
                                                          new ContractDetails(
                                                              x.EmploymentType,
                                                              new SalaryRange(
                                                                  x.SalaryRange.IsPublished,
                                                                  x.SalaryRange.IsPublished ? x.SalaryRange.From : 0,
                                                                  x.SalaryRange.IsPublished ? x.SalaryRange.To : 0),
                                                              x.TimeNumerator,
                                                              x.TimeDenominator
                                                          )).ToList());

    public static PublishedJobOfferListItem MapToJobOfferListItem(this PublishedJobOffer publishedJobOffer) =>
        new(publishedJobOffer.Id.Guid,
            publishedJobOffer.OfferSummary,
            publishedJobOffer.RemoteWork,
            publishedJobOffer.ContractsDetails.Select(x =>
                                                          new ContractDetails(
                                                              x.EmploymentType,
                                                              new SalaryRange(
                                                                  x.SalaryRange.IsPublished,
                                                                  x.SalaryRange.IsPublished ? x.SalaryRange.From : 0,
                                                                  x.SalaryRange.IsPublished ? x.SalaryRange.To : 0),
                                                              x.TimeNumerator,
                                                              x.TimeDenominator
                                                          )).ToList(),
            publishedJobOffer.Location.City,
            publishedJobOffer.Location.Country,
            publishedJobOffer.CompanyDetails.Name,
            publishedJobOffer.PublishedAt);
}