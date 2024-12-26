using Heyer.BuildingBlocks.Application.Results;
using Heyer.Modules.Hiring.PublishedLanguage.DTOs;
using Heyer.Modules.JobBoard.Application.JobOffers.PublicJobOfferDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Heyer.Modules.JobBoard.Application;

public static class JobBoardEndpointsConfiguration
{
    public static void MapEndpoints(WebApplication app) => MapGetJobOfferDetailsEndpoint(app);

    private static void MapGetJobOfferDetailsEndpoint(WebApplication app) =>
        app.MapGet("/job-board/{jobOfferId}",
                   async (IJobBoardModule module, Guid jobOfferId, CancellationToken cancellationToken) =>
                   {
                       var result =
                           await module.DispatchQuery<GetPublicJobOfferDetails, PublishedJobOfferDetails>(
                               new GetPublicJobOfferDetails(jobOfferId),
                               cancellationToken);

                       return result.IsSuccess
                           ? Results.Ok(result.Value)
                           : ResponseErrorHandling.Handle(result);
                   });
}