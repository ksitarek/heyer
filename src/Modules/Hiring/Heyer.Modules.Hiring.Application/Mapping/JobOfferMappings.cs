using Heyer.Modules.Hiring.Application.JobOffers.ContractDetails.AddContractDetails;
using Heyer.Modules.Hiring.Application.JobOffers.ContractDetails.RemoveContractDetails;
using Heyer.Modules.Hiring.Application.JobOffers.ContractDetails.UpdateContractDetails;
using Heyer.Modules.Hiring.Application.JobOffers.Create;
using Heyer.Modules.Hiring.Application.JobOffers.Publish;
using Heyer.Modules.Hiring.Application.JobOffers.SetOfficeLocation;
using Heyer.Modules.Hiring.Application.JobOffers.Update;
using Heyer.Modules.Hiring.Domain.JobOffers;
using Heyer.Modules.Hiring.PublishedLanguage.DTOs;

namespace Heyer.Modules.Hiring.Application.Mapping;

public static class JobOfferMappings
{
    public static PublishJobOffer MapToCommand(this PublishJobOfferRequest request) =>
        new(request.JobOfferId, request.PublishedUntil);

    public static CreateJobOffer MapToCommand(this CreateJobOfferRequest request) =>
        new(
            request.OfferSummary,
            request.JobDescription,
            request.RemoteWork);

    public static AddContractDetails MapToCommand(this AddContractDetailsRequest request) =>
        new(new JobOfferId(request.JobOfferId), request.ContractDetails);

    public static RemoveContractDetails MapToCommand(this RemoveContractDetailsRequest request) =>
        new(new JobOfferId(request.JobOfferId), request.EmploymentType);

    public static UpdateContractDetails MapToCommand(this UpdateContractDetailsRequest request) =>
        new(new JobOfferId(request.JobOfferId),
            request.EmploymentType,
            request.SalaryRange,
            request.TimeNumerator,
            request.TimeDenominator);

    public static SetOfficeLocation MapToCommand(this SetOfficeLocationRequest request) =>
        new(new JobOfferId(request.JobOfferId), new OfficeLocation(request.City, request.Country));

    public static UpdateJobOffer MapToCommand(this UpdateJobOfferRequest request) => new(
        new JobOfferId(request.JobOfferId),
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