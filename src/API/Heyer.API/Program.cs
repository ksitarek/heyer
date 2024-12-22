using Heyer.API.HealthChecks;
using Heyer.BuildingBlocks.Application.Authorization;
using Heyer.BuildingBlocks.Infrastructure.HealthChecks;
using Heyer.BuildingBlocks.Infrastructure.Modules;
using Heyer.Modules.Hiring.Application;
using Heyer.Modules.Hiring.Infrastructure;
using Heyer.Modules.JobBoard.Application;
using Heyer.Modules.JobBoard.Infrastructure;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSerilog((sp, lc) => lc
                                .ReadFrom.Configuration(builder.Configuration)
                                .ReadFrom.Services(sp), true);

builder.Services.AddModule<IHiringModule, HiringModule>();
builder.Services.AddModule<IJobBoardModule, JobBoardModule>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthcheck>("Database", timeout: TimeSpan.FromSeconds(3));

builder.Services.AddAuthenticationAndAuthorization(builder.Configuration.GetSection("Jwt"));

var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseAuthentication();
app.UseAuthorization();
app.UseModules();
app.UseHealthChecks("/health", new HealthCheckOptions { ResponseWriter = JsonResponseWriter.WriteResponse });

await app.RunAsync();

namespace Heyer.API
{
    public class Program
    {
    }
}