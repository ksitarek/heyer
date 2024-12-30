using System.Reflection;
using FluentValidation;
using Heyer.BuildingBlocks.Infrastructure;
using Heyer.BuildingBlocks.Infrastructure.HealthChecks;
using Heyer.BuildingBlocks.Infrastructure.Mediator;
using Heyer.BuildingBlocks.Infrastructure.Mediator.Middleware;
using Heyer.Storage.API.BackgroundTasks;
using Heyer.Storage.API.Extensions;
using Heyer.Storage.API.HealthChecks;
using Heyer.Storage.API.Providers.Registry;
using Heyer.Storage.API.Providers.Storage;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAntiforgery();

builder.Services.AddHealthChecks()
    .AddCheck<StorageHealthcheck>("Storage", timeout: TimeSpan.FromSeconds(3))
    .AddCheck<RegistryHealthcheck>("Registry", timeout: TimeSpan.FromSeconds(3));

builder.Services.AddSingleton<IDateTimeProvider, SystemDateTime>();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
builder.Services.AddMediator(
    typeof(LoggingMiddleware<,>),
    typeof(ValidationMiddleware<,>));

builder.Services.AddCleanupService(builder.Configuration.GetSection("CleanupService"));
builder.Services.AddJwtAuthentication(builder.Configuration.GetSection("Jwt"));
builder.Services.AddStorageStrategy(builder.Configuration.GetSection("StorageStrategy"));
builder.Services.AddRegistryStrategy(builder.Configuration.GetSection("RegistryStrategy"));

var app = builder.Build();

app.UseAntiforgery();

app.MapEndpoints();
app.UseHealthChecks("/health", new HealthCheckOptions { ResponseWriter = JsonResponseWriter.WriteResponse });

app.EnsureDatabaseIsCreated();

await app.RunAsync();

// For Integration Testing
namespace Heyer.Storage.API
{
    public class Program
    {
    };
}