using Heyer.Storage.API.Endpoints.Store;
using MediatR;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;

namespace Heyer.Storage.API.Endpoints;

public static class EndpointsExtensions
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        return app.MapStoreEndpoint()
            .MapAntiforgeryEndpoint();
    }

    private static WebApplication MapStoreEndpoint(this WebApplication app)
    {
        app.MapPost("/store", async (IMediator mediator, [FromForm] StoreRequest request) =>
        {
            var response = await mediator.Send(request);
            return response.IsSuccess
                ? Results.Ok(response.ValueOrDefault)
                : ResponseErrorHandling.Handle(response);
        });

        return app;
    }

    private static WebApplication MapAntiforgeryEndpoint(this WebApplication app)
    {
        app.MapGet("csrf", (IAntiforgery forgeryService, HttpContext context) =>
        {
            var tokens = forgeryService.GetAndStoreTokens(context);
            var xsrfToken = tokens.RequestToken!;
            return TypedResults.Content(xsrfToken, "text/plain");
        });

        return app;
    }
}