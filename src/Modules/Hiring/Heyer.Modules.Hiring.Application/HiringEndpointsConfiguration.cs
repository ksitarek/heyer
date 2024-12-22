using Heyer.BuildingBlocks.Application.Results;
using Heyer.Modules.Hiring.Application.Candidates.NewCandidateApply;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Heyer.Modules.Hiring.Application;

public class HiringEndpointsConfiguration
{
    public HiringEndpointsConfiguration(WebApplication app) => MapNewCandidateApplyEndpoint(app);

    private static void MapNewCandidateApplyEndpoint(WebApplication app) =>
        app.MapPost("/job-offers/new-candidate-apply",
                    async (IMediator mediator, [FromBody] NewCandidateApplyToJobOffer command) =>
                    {
                        var result = await mediator.Send(command);

                        return result.IsSuccess
                            ? Results.Ok()
                            : ResponseErrorHandling.Handle(result);
                    });
}