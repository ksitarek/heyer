using Heyer.API.Client;
using Heyer.API.Client.PublishedLanguage;
using Heyer.BuildingBlocks.Application.Authorization;
using Heyer.BuildingBlocks.Application.Results;
using Heyer.Modules.JobBoard.Application.JobOffers.Create;
using Heyer.Modules.JobBoard.Application.JobOffers.NewCandidateApply;
using Heyer.Modules.JobBoard.Application.Mapping;
using Heyer.Modules.JobBoard.Domain.Companies;
using Heyer.Modules.JobBoard.Domain.JobOffers;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RemoteWork = Heyer.Modules.JobBoard.Domain.JobOffers.RemoteWork;

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
        app.MapPost("/job-offers/create", async (IMediator mediator, [FromBody] CreateJobOfferRequest request) =>
        {
            var command = request.MapToCommand();
            
            var result = await mediator.Send(command);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : ResponseErrorHandling.Handle(result);
        }).RequirePermission(JobBoardPermissions.CreateJobOffer);
    }

    private static void MapNewCandidateApplyEndpoint(WebApplication app)
    {
        app.MapPost("/job-offers/new-candidate-apply", async (IMediator mediator, [FromBody] NewCandidateApplyToJobOffer command) =>
        {
            var result = await mediator.Send(command);
            
            return result.IsSuccess
                ? Results.Ok()
                : ResponseErrorHandling.Handle(result);
        });
    }
    
    private static RemoteWork MapRemoteWork(Heyer.API.Client.PublishedLanguage.RemoteWork remoteWork) =>
        remoteWork switch
        {
            Heyer.API.Client.PublishedLanguage.RemoteWork.No => RemoteWork.No,
            Heyer.API.Client.PublishedLanguage.RemoteWork.Hybrid => RemoteWork.Hybrid,
            Heyer.API.Client.PublishedLanguage.RemoteWork.Yes => RemoteWork.Yes,
            _ => throw new ArgumentOutOfRangeException(nameof(remoteWork))
        };
}