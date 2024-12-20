using Heyer.API.HealthChecks;
using Heyer.BuildingBlocks.Application.Authorization;
using Heyer.BuildingBlocks.Infrastructure;
using Heyer.BuildingBlocks.Infrastructure.HealthChecks;
using Heyer.BuildingBlocks.Infrastructure.Mediator;
using Heyer.BuildingBlocks.Infrastructure.Mediator.Middleware;
using Heyer.BuildingBlocks.Infrastructure.Messaging;
using Heyer.BuildingBlocks.Infrastructure.Modules;
using Heyer.Modules.JobBoard.Infrastructure;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

var modules = new IModule[] { new JobBoardModule(builder.Configuration) };

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthcheck>("Database", timeout: TimeSpan.FromSeconds(3));

builder.Services.AddSingleton<IDateTimeProvider, SystemDateTime>();
builder.Services.AddMediator(modules,
                             typeof(LoggingMiddleware<,>),
                             typeof(ValidationMiddleware<,>),
                             typeof(UnitOfWorkMiddleware<,>));

builder.Services.AddAuthenticationAndAuthorization(builder.Configuration.GetSection("Jwt"));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddDomainEventDispatcher();

builder.AddModules(modules);

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.UseModules(modules);
app.UseHealthChecks("/health", new HealthCheckOptions { ResponseWriter = JsonResponseWriter.WriteResponse });

await app.RunAsync();

namespace Heyer.API
{
    public class Program
    {
    }
}