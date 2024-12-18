using FluentValidation;
using Heyer.BuildingBlocks.Infrastructure.Modules;
using Heyer.Modules.JobBoard.Application;
using Heyer.Storage.API.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Heyer.Modules.JobBoard.Infrastructure;

public class JobBoardModule : IModule
{
    private readonly IConfiguration _configuration;

    public JobBoardModule(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureDependencyInjection(IServiceCollection services)
    {
        services.AddStorageApiClient(_configuration["StorageApi:Url"]);
        services.AddValidatorsFromAssemblyContaining<JobBoardEndpointsConfiguration>();
    }

    public void ConfigureModule(WebApplication app)
    {
        var endpointsConfiguration = new JobBoardEndpointsConfiguration();
        endpointsConfiguration.MapJobBoardEndpoints(app);
    }
}