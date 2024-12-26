using Heyer.Modules.Hiring.Application;
using Heyer.Modules.Hiring.Infrastructure;
using Heyer.Modules.JobBoard.Application;
using Heyer.Modules.JobBoard.Infrastructure;
using Serilog;
using HostBuilder = Heyer.API.Host.HostBuilder;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = new HostBuilder(WebApplication.CreateBuilder(args));

builder
    .ConfigureLogging()
    .AddModule<IHiringModule, HiringModule>()
    .AddModule<IJobBoardModule, JobBoardModule>()
    .ConfigureHealthChecks();
var host = builder.Build();

await host.RunAsync();

namespace Heyer.API
{
    public class Program
    {
    }
}