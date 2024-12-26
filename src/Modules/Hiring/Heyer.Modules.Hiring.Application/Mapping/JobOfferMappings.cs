using Heyer.Modules.Hiring.Application.JobOffers.Create;
using Heyer.Modules.Hiring.Domain.JobOffers;
using Heyer.Modules.Hiring.PublishedLanguage.DTOs;

namespace Heyer.Modules.Hiring.Application.Mapping;

public static class JobOfferMappings
{
    public static CreateJobOffer MapToCommand(this CreateJobOfferRequest request) =>
        new(
            request.OfferSummary,
            request.JobDescription,
            request.RemoteWork);

    public static JobOfferDetails MapToJobOfferDetails(this JobOffer jobOffer) =>
        new(jobOffer.Id.Guid,
            jobOffer.OfferSummary,
            jobOffer.JobDescription,
            jobOffer.PublishedAt,
            jobOffer.PublishedUntil,
            jobOffer.Location!,
            jobOffer.RemoteWork,
            jobOffer.Requirements!,
            jobOffer.ContractsDetails!.ToList());
}