using Heyer.API.Client.PublishedLanguage;
using Heyer.BuildingBlocks.Application.Authorization;
using Heyer.BuildingBlocks.Application.Results;
using Heyer.Modules.Hiring.Application.Candidates.NewCandidateApply;
using Heyer.Modules.Hiring.Application.JobOffers.Create;
using Heyer.Modules.Hiring.Application.Mapping;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Heyer.Modules.Hiring.Application;

public static class HiringEndpointsConfiguration
{
    public static void MapEndpoints(WebApplication app)
    {
        MapCreateJobOfferEndpoint(app);
        MapNewCandidateApplyEndpoint(app);
    }

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
}