using Heyer.BuildingBlocks.Application.Authorization;
using Heyer.Modules.JobBoard.Application.JobOffers.Create;
using Heyer.Modules.JobBoard.Application.JobOffers.NewCandidateApply;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Heyer.Modules.JobBoard.Application;

public class JobBoardEndpointsConfiguration
{
    public void MapJobBoardEndpoints(WebApplication app)
    {
        MapCreateJobOfferEndpoint(app);
        MapNewCandidateApplyEndpoint(app);
    }

    private static void MapCreateJobOfferEndpoint(WebApplication app)
    {
        app.MapPost("/job-offers/create", async (IMediator mediator, [FromBody] CreateJobOffer command) =>
        {
            var result = await mediator.Send(command);
            
            return result.IsSuccess
                ? Results.Ok()
                : Results.StatusCode(500); // TODO
        }).RequirePermission(JobBoardPermissions.CreateJobOffer);
    }

    private static void MapNewCandidateApplyEndpoint(WebApplication app)
    {
        app.MapPost("/job-offers/new-candidate-apply", async (IMediator mediator, [FromBody] NewCandidateApplyToJobOffer command) =>
        {
            var result = await mediator.Send(command);
            
            return result.IsSuccess
                ? Results.Ok()
                : Results.StatusCode(500); // TODO
        });
    }
}