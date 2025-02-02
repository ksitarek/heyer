using Heyer.BuildingBlocks.Application.Authorization;
using Heyer.BuildingBlocks.Application.HttpLanguage;
using Heyer.BuildingBlocks.Application.Results;
using Heyer.Modules.Hiring.Application.Candidates.NewCandidateApply;
using Heyer.Modules.Hiring.Application.JobOffers.CheckForConflicts;
using Heyer.Modules.Hiring.Application.JobOffers.Create;
using Heyer.Modules.Hiring.Application.JobOffers.GetById;
using Heyer.Modules.Hiring.Application.JobOffers.List;
using Heyer.Modules.Hiring.Application.Mapping;
using Heyer.Modules.Hiring.Domain.JobOffers;
using Heyer.Modules.Hiring.PublishedLanguage.DTOs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Heyer.Modules.Hiring.Application;

public static class HiringEndpointsConfiguration
{
    public static void MapEndpoints(WebApplication app)
    {
        MapAddContractDetailsEndpoint(app);
        MapCheckForConflictsEndpoint(app);
        MapCreateJobOfferEndpoint(app);
        MapGetJobOfferByIdEndpoint(app);
        MapGetJobOffersListEndpoint(app);
        MapNewCandidateApplyEndpoint(app);
        MapPublishJobOfferEndpoint(app);
        MapRemoveContractDetailsEndpoint(app);
        MapSetOfficeLocationEndpoint(app);
        MapSetRequirementsEndpoint(app);
        MapTakeDownJobOfferEndpoint(app);
        MapUpdateContractDetailsEndpoint(app);
        MapUpdateJobOfferEndpoint(app);
    }

    private static void MapAddContractDetailsEndpoint(WebApplication app) =>
        app.MapPost("/job-offers/add-contract-details",
                    async (IHiringModule module,
                           AddContractDetailsRequest request,
                           CancellationToken cancellationToken) =>
                    {
                        var command = request.MapToCommand();

                        var result = await module.DispatchCommand(command, cancellationToken);

                        return result.IsSuccess
                            ? Results.Ok()
                            : ResponseErrorHandling.Handle(result);
                    }).RequirePermission(HiringPermissions.UpdateJobOffer);

    private static void MapCheckForConflictsEndpoint(WebApplication app) =>
        app.MapGet("/job-offers/{jobOfferId}/check-for-conflicts",
                   async (IHiringModule module,
                          [FromRoute] Guid jobOfferId,
                          CancellationToken cancellationToken) =>
                   {
                       var query = new CheckForConflicts(new JobOfferId(jobOfferId));
                       var result = await module.DispatchQuery<CheckForConflicts, bool>(query, cancellationToken);

                       return result.IsSuccess
                           ? Results.Ok(result.Value)
                           : ResponseErrorHandling.Handle(result);
                   }).RequirePermission(HiringPermissions.UpdateJobOffer);

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
                    }).RequirePermission(HiringPermissions.UpdateJobOffer);

    private static void MapGetJobOfferByIdEndpoint(WebApplication app) =>
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

    private static void MapGetJobOffersListEndpoint(WebApplication app) =>
        app.MapGet("/job-offers",
                   async (IHiringModule module,
                          [FromQuery] int page,
                          [FromQuery] int pageSize,
                          [FromQuery] string sortBy,
                          [FromQuery] string sortOrder,
                          CancellationToken cancellationToken) =>
                   {
                       var result =
                           await module.DispatchQuery<GetJobOffersList, ListResponse<JobOfferListItem>>(
                               new GetJobOffersList(page,
                                                    pageSize,
                                                    SortRequest.From(sortBy, sortOrder)),
                               cancellationToken);

                       return result.IsSuccess
                           ? Results.Ok(result.Value)
                           : ResponseErrorHandling.Handle(result);
                   }).RequirePermission(HiringPermissions.ListJobOffers);

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
        app.MapPost("/job-offers/publish",
                    async (IHiringModule module,
                           PublishJobOfferRequest request,
                           CancellationToken cancellationToken = default) =>
                    {
                        var command = request.MapToCommand();

                        var result = await module.DispatchCommand(
                            command,
                            cancellationToken);

                        return result.IsSuccess
                            ? Results.Ok()
                            : ResponseErrorHandling.Handle(result);
                    }).RequirePermission(HiringPermissions.PublishJobOffer);

    private static void MapRemoveContractDetailsEndpoint(WebApplication app) =>
        app.MapPost("/job-offers/remove-contract-details",
                    async (IHiringModule module,
                           RemoveContractDetailsRequest request,
                           CancellationToken cancellationToken) =>
                    {
                        var command = request.MapToCommand();

                        var result = await module.DispatchCommand(command, cancellationToken);

                        return result.IsSuccess
                            ? Results.Ok()
                            : ResponseErrorHandling.Handle(result);
                    }).RequirePermission(HiringPermissions.UpdateJobOffer);

    private static void MapSetOfficeLocationEndpoint(WebApplication app) =>
        app.MapPost("/job-offers/set-office-location",
                    async (IHiringModule module,
                           SetOfficeLocationRequest request,
                           CancellationToken cancellationToken) =>
                    {
                        var command = request.MapToCommand();

                        var result = await module.DispatchCommand(command, cancellationToken);

                        return result.IsSuccess
                            ? Results.Ok()
                            : ResponseErrorHandling.Handle(result);
                    }).RequirePermission(HiringPermissions.UpdateJobOffer);

    private static void MapSetRequirementsEndpoint(WebApplication app) =>
        app.MapPost("/job-offers/set-requirements",
                    async (IHiringModule module,
                           SetRequirementsRequest request,
                           CancellationToken cancellationToken) =>
                    {
                        var command = request.MapToCommand();

                        var result = await module.DispatchCommand(command, cancellationToken);

                        return result.IsSuccess
                            ? Results.Ok()
                            : ResponseErrorHandling.Handle(result);
                    }).RequirePermission(HiringPermissions.UpdateJobOffer);

    private static void MapTakeDownJobOfferEndpoint(WebApplication app) =>
        app.MapPost("/job-offers/take-down",
                    async (IHiringModule module,
                           TakeDownRequest request,
                           CancellationToken cancellationToken) =>
                    {
                        var command = request.MapToCommand();

                        var result = await module.DispatchCommand(command, cancellationToken);

                        return result.IsSuccess
                            ? Results.Ok()
                            : ResponseErrorHandling.Handle(result);
                    }).RequirePermission(HiringPermissions.TakeDownJobOffer);

    private static void MapUpdateContractDetailsEndpoint(WebApplication app) =>
        app.MapPost("/job-offers/update-contract-details",
                    async (IHiringModule module,
                           UpdateContractDetailsRequest request,
                           CancellationToken cancellationToken) =>
                    {
                        var command = request.MapToCommand();

                        var result = await module.DispatchCommand(command, cancellationToken);

                        return result.IsSuccess
                            ? Results.Ok()
                            : ResponseErrorHandling.Handle(result);
                    }).RequirePermission(HiringPermissions.UpdateJobOffer);

    private static void MapUpdateJobOfferEndpoint(WebApplication app) =>
        app.MapPost("/job-offers/update",
                    async (IHiringModule module,
                           UpdateJobOfferRequest request,
                           CancellationToken cancellationToken) =>
                    {
                        var command = request.MapToCommand();

                        var result = await module.DispatchCommand(command, cancellationToken);

                        return result.IsSuccess
                            ? Results.Ok()
                            : ResponseErrorHandling.Handle(result);
                    }).RequirePermission(HiringPermissions.UpdateJobOffer);
}