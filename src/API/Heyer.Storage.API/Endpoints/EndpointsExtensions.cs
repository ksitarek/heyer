using Heyer.Storage.API.Client.PublishedLanguage;
using MediatR;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;

namespace Heyer.Storage.API.Endpoints;

public static class EndpointsExtensions
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        return app
            .MapDeleteEndpoint()
            .MapDownloadEndpoint()
            .MapPreserveEndpoint()
            .MapStoreEndpoint()
            .MapAntiforgeryEndpoint();
    }

    private static WebApplication MapDeleteEndpoint(this WebApplication app)
    {
        app.MapDelete("/delete/{Key}", async (IMediator mediator, [AsParameters] DeleteRequest request) =>
        {
            var response = await mediator.Send(request);
            return response.IsSuccess
                ? Results.Ok()
                : ResponseErrorHandling.Handle(response);
        }).RequireAuthorization();

        return app;
    }

    private static WebApplication MapDownloadEndpoint(this WebApplication app)
    {
        app.MapGet("/download/{Key}", async (IMediator mediator, [AsParameters] DownloadRequest request) =>
        {
            var response = await mediator.Send(request);
            return response.IsSuccess
                ? Results.File(response.Value.FileContent, response.Value.ContentType, response.Value.FileName)
                : ResponseErrorHandling.Handle(response);
        }).RequireAuthorization();

        return app;
    }

    private static WebApplication MapPreserveEndpoint(this WebApplication app)
    {
        app.MapPost("/preserve/{key}", async (IMediator mediator, [AsParameters] PreserveRequest request) =>
        {
            var response = await mediator.Send(request);
            return response.IsSuccess
                ? Results.Ok()
                : ResponseErrorHandling.Handle(response);
        });

        return app;
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