using Heyer.BuildingBlocks.Application.Results;
using Heyer.Modules.Hiring.Application.Candidates.NewCandidateApply;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Heyer.Modules.Hiring.Application;

public static class HiringEndpointsConfiguration
{
    public static void MapEndpoints(WebApplication app) => MapNewCandidateApplyEndpoint(app);

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
}