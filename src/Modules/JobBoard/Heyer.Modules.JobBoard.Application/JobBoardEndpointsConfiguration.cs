using Heyer.BuildingBlocks.Application.Results;
using Heyer.Modules.Hiring.PublishedLanguage.DTOs;
using Heyer.Modules.JobBoard.Application.JobOffers.List;
using Heyer.Modules.JobBoard.Application.JobOffers.PublicJobOfferDetails;
using Heyer.Modules.JobBoard.Application.Mapping;
using Heyer.Modules.JobBoard.PublishedLanguage.DTOs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Heyer.Modules.JobBoard.Application;

public static class JobBoardEndpointsConfiguration
{
    public static void MapEndpoints(WebApplication app)
    {
        MapGetJobOfferDetailsEndpoint(app);
        MapGetJobOffersListEndpoint(app);
        MapNewCandidateApplyEndpoint(app);
    }

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

    private static void MapGetJobOffersListEndpoint(WebApplication app) =>
        app.MapGet("/job-board",
                   async (IJobBoardModule module, CancellationToken cancellationToken) =>
                   {
                       var result =
                           await module.DispatchQuery<GetList, IEnumerable<PublishedJobOfferListItem>>(
                               new GetList(),
                               cancellationToken);

                       return result.IsSuccess
                           ? Results.Ok(result.Value)
                           : ResponseErrorHandling.Handle(result);
                   });

    private static void MapNewCandidateApplyEndpoint(WebApplication app) =>
        app.MapPost("/job-offers/new-candidate-apply",
                    async (IJobBoardModule module,
                           [FromBody] NewCandidateApplyToJobOfferRequest request,
                           CancellationToken cancellationToken) =>
                    {
                        var command = request.MapToCommand();

                        var result = await module.DispatchCommand(command, cancellationToken);

                        return result.IsSuccess
                            ? Results.Ok()
                            : ResponseErrorHandling.Handle(result);
                    });
}