using Heyer.API.Client.PublishedLanguage;
using Heyer.BuildingBlocks.Application.Authorization;
using Heyer.BuildingBlocks.Application.Results;
using Heyer.Modules.JobBoard.Application.JobOffers.Create;
using Heyer.Modules.JobBoard.Application.JobOffers.PublicJobOfferDetails;
using Heyer.Modules.JobBoard.Application.Mapping;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Heyer.Modules.JobBoard.Application;

public static class JobBoardEndpointsConfiguration
{
    public static void MapEndpoints(WebApplication app)
    {
        MapCreateJobOfferEndpoint(app);
        MapGetJobOfferDetailsEndpoint(app);
    }

    private static void MapCreateJobOfferEndpoint(WebApplication app) =>
        app.MapPost("/job-offers/create",
                    async (IJobBoardModule module, [FromBody] CreateJobOfferRequest request, CancellationToken cancellationToken) =>
                    {
                        var command = request.MapToCommand();

                        var result = await module.DispatchCommand<CreateJobOffer, Guid>(command, cancellationToken);

                        return result.IsSuccess
                            ? Results.Ok(result.Value)
                            : ResponseErrorHandling.Handle(result);
                    }).RequirePermission(JobBoardPermissions.CreateJobOffer);

    private static void MapGetJobOfferDetailsEndpoint(WebApplication app) =>
        app.MapGet("/job-offers/{jobOfferId}",
                   async (IJobBoardModule module, Guid jobOfferId, CancellationToken cancellationToken) =>
                   {
                       var result = await module.DispatchQuery<GetPublicJobOfferDetails, JobOfferDetails>(new GetPublicJobOfferDetails(jobOfferId), cancellationToken);

                       return result.IsSuccess
                           ? Results.Ok(result.Value)
                           : ResponseErrorHandling.Handle(result);
                   });
}