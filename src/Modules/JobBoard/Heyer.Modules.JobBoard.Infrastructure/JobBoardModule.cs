using System.Reflection;
using FluentValidation;
using Heyer.BuildingBlocks.Infrastructure;
using Heyer.BuildingBlocks.Infrastructure.Modules;
using Heyer.Modules.JobBoard.Application;
using Heyer.Modules.JobBoard.Domain.Candidates;
using Heyer.Modules.JobBoard.Domain.JobOffers;
using Heyer.Modules.JobBoard.Infrastructure.Persistence;
using Heyer.Storage.API.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Heyer.Modules.JobBoard.Infrastructure;

public class JobBoardModule : IModule
{
    private readonly IConfiguration _configuration;

    public JobBoardModule(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Assembly ModuleApplicationAssembly => typeof(JobBoardEndpointsConfiguration).Assembly;

    public void ConfigureDependencyInjection(IServiceCollection services)
    {
        services.AddStorageApiClient(_configuration["StorageApi:Url"]);
        services.AddValidatorsFromAssemblyContaining<JobBoardEndpointsConfiguration>();

        var client = new MongoClient(_configuration["MongoDb:ConnectionString"]!);
        var db = client.GetDatabase(_configuration["MongoDb:DatabaseName"]!);

        services.AddDbContext<JobBoardContext>(o => 
            o.UseMongoDB(db.Client, db.DatabaseNamespace.DatabaseName));

        services.AddScoped<DbContext>((sp) => sp.GetRequiredService<JobBoardContext>());

        services.AddScoped<ICandidatesRepository, CandidatesRepository>();
        services.AddScoped<IJobOffersRepository, JobOffersRepository>();
    }

    public void ConfigureModule(WebApplication app)
    {
        var endpointsConfiguration = new JobBoardEndpointsConfiguration();
        endpointsConfiguration.MapJobBoardEndpoints(app);
    }
}