using Heyer.API.Client.PublishedLanguage;
using Heyer.Modules.JobBoard.Application.JobOffers.Create;
using Heyer.Modules.JobBoard.Domain.JobOffers;
using RemoteWork = Heyer.Modules.JobBoard.Domain.JobOffers.RemoteWork;

namespace Heyer.Modules.JobBoard.Application.Mapping;

public static class JobOfferMappings
{
    public static CreateJobOffer MapToCommand(this CreateJobOfferRequest request) =>
        new(
            request.OfferSummary,
            request.JobDescription,
            request.RemoteWork.MapRemoteWork());

    public static JobOfferDetails MapToJobOfferDetails(this JobOffer jobOffer) =>
        new(jobOffer.Id.Guid, jobOffer.GetOfferSummary(), jobOffer.GetJobDescription());

    private static RemoteWork MapRemoteWork(this API.Client.PublishedLanguage.RemoteWork remoteWork) =>
        remoteWork switch
        {
            API.Client.PublishedLanguage.RemoteWork.Unknown => RemoteWork.Unknown,
            API.Client.PublishedLanguage.RemoteWork.No => RemoteWork.No,
            API.Client.PublishedLanguage.RemoteWork.Hybrid => RemoteWork.Hybrid,
            API.Client.PublishedLanguage.RemoteWork.Yes => RemoteWork.Yes,
            _ => throw new ArgumentOutOfRangeException(nameof(remoteWork), remoteWork, null)
        };
}