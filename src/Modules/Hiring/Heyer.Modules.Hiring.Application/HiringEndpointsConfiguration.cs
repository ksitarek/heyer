using Heyer.BuildingBlocks.Application.Authorization;
using Heyer.BuildingBlocks.Application.Results;
using Heyer.Modules.Hiring.Application.Candidates.NewCandidateApply;
using Heyer.Modules.Hiring.Application.JobOffers.Create;
using Heyer.Modules.Hiring.Application.JobOffers.GetById;
using Heyer.Modules.Hiring.Application.JobOffers.Publish;
using Heyer.Modules.Hiring.Application.Mapping;
using Heyer.Modules.Hiring.PublishedLanguage.DTOs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Heyer.Modules.Hiring.Application;

public static class HiringEndpointsConfiguration
{
    public static void MapEndpoints(WebApplication app)
    {
        MapCreateJobOfferEndpoint(app);
        MpaGetJobOfferByIdEndpoint(app);
        MapNewCandidateApplyEndpoint(app);
        MapPublishJobOfferEndpoint(app);
    }


    private static void MapCreateJobOfferEndpoint(WebApplication app) =>
        app.MapPost("/job-offers/create",
                    async (IHiringModule module,
                           [FromBody] CreateJobOfferRequest request,
                           CancellationToken cancellationToken) =>
                    {
                        var command = request.MapToCommand();

                        var result = await module.DispatchCommand<CreateJobOffer, Guid>(command, cancellationToken);

                        return result.IsSuccess
                            ? Results.Ok(result.Value)
                            : ResponseErrorHandling.Handle(result);
                    }).RequirePermission(HiringPermissions.CreateJobOffer);

    private static void MapNewCandidateApplyEndpoint(WebApplication app) =>
        app.MapPost("/job-offers/new-candidate-apply",
                    async (IHiringModule module,
                           [FromBody] NewCandidateApplyToJobOffer command,
                           CancellationToken cancellationToken) =>
                    {
                        var result = await module.DispatchCommand(command, cancellationToken);

                        return result.IsSuccess
                            ? Results.Ok()
                            : ResponseErrorHandling.Handle(result);
                    });

    private static void MapPublishJobOfferEndpoint(WebApplication app) =>
        app.MapPost("/job-offers/publish/{jobOfferId}",
                    async (IHiringModule module,
                           [FromRoute] Guid jobOfferId,
                           CancellationToken cancellationToken) =>
                    {
                        var result = await module.DispatchCommand(
                            new PublishJobOffer(jobOfferId),
                            cancellationToken);

                        return result.IsSuccess
                            ? Results.Ok()
                            : ResponseErrorHandling.Handle(result);
                    }).RequirePermission(HiringPermissions.PublishJobOffer);

    private static void MpaGetJobOfferByIdEndpoint(WebApplication app) =>
        app.MapGet("/job-offers/{jobOfferId}",
                   async (IHiringModule module,
                          [FromRoute] Guid jobOfferId,
                          CancellationToken cancellationToken) =>
                   {
                       var result =
                           await module.DispatchQuery<GetJobOfferById, JobOfferDetails>(
                               new GetJobOfferById(jobOfferId),
                               cancellationToken);

                       return result.IsSuccess
                           ? Results.Ok(result.Value)
                           : ResponseErrorHandling.Handle(result);
                   }).RequirePermission(HiringPermissions.ListJobOffers);
}