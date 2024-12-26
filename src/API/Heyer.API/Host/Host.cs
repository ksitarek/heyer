using Hangfire;
using Heyer.BuildingBlocks.Infrastructure.HealthChecks;
using Heyer.BuildingBlocks.Infrastructure.Modules;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;

namespace Heyer.API.Host;

internal class Host
{
    private readonly WebApplication _app;

    public Host(WebApplication app)
    {
        _app = app;

        _app.UseSerilogRequestLogging();
        _app.UseAuthentication();
        _app.UseAuthorization();
        _app.UseModules();
        _app.UseHealthChecks("/health", new HealthCheckOptions { ResponseWriter = JsonResponseWriter.WriteResponse });
        _app.UseHangfireDashboard();
    }

    public async Task RunAsync() => await _app.RunAsync();
}