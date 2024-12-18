using Heyer.API.Client.PublishedLanguage;
using Heyer.Modules.JobBoard.Application.JobOffers.Create;
using Heyer.Modules.JobBoard.Domain.Companies;
using Heyer.Modules.JobBoard.Domain.JobOffers;
using RemoteWork = Heyer.Modules.JobBoard.Domain.JobOffers.RemoteWork;

namespace Heyer.Modules.JobBoard.Application.Mapping;

public static class JobOfferMappings
{
    public static CreateJobOffer MapToCommand(this CreateJobOfferRequest request) =>
        new(
            new CompanyDetails(CompanyId.CreateNew(), "test-company"), // TODO: Get company details from the user
            request.OfferSummary,
            request.JobDescription,
            request.RemoteWork.MapRemoteWork());
    
    private static RemoteWork MapRemoteWork(this API.Client.PublishedLanguage.RemoteWork remoteWork) =>
        remoteWork switch
        {
            API.Client.PublishedLanguage.RemoteWork.No => RemoteWork.No,
            API.Client.PublishedLanguage.RemoteWork.Hybrid => RemoteWork.Hybrid,
            API.Client.PublishedLanguage.RemoteWork.Yes => RemoteWork.Yes,
            _ => throw new ArgumentOutOfRangeException(nameof(remoteWork), remoteWork, null)
        };
}