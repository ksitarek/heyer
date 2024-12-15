using System.Reflection;
using FluentValidation;
using Heyer.BuildingBlocks.Infrastructure.Modules;
using Heyer.Modules.Candidates.Application.Candidates.Create;
using Heyer.Storage.API.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Heyer.Modules.Candidates.Infrastructure;

public class CandidatesModule : IModule
{
    private readonly IConfiguration _configuration;

    public CandidatesModule(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureDependencyInjection(IServiceCollection services)
    {
        services.AddStorageApiClient(_configuration["StorageApi:Url"]);
        services.AddValidatorsFromAssemblyContaining<CreateCandidate>();
    }

    public void ConfigureModule(IApplicationBuilder app)
    {
    }

}