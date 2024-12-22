using System.Reflection;
using FluentValidation;
using Heyer.BuildingBlocks.Application.Authorization;
using Heyer.BuildingBlocks.Infrastructure;
using Heyer.BuildingBlocks.Infrastructure.Mediator;
using Heyer.BuildingBlocks.Infrastructure.Mediator.Middleware;
using Heyer.BuildingBlocks.Infrastructure.Messaging;
using Heyer.BuildingBlocks.Infrastructure.Modules;
using Heyer.Modules.JobBoard.Application;
using Heyer.Modules.JobBoard.Domain.JobOffers;
using Heyer.Modules.JobBoard.Infrastructure.Configuration;
using Heyer.Modules.JobBoard.Infrastructure.Persistence;
using Heyer.Storage.API.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Heyer.Modules.JobBoard.Infrastructure;

public class JobBoardModule : ModuleRunner, IJobBoardModule, IModuleInstaller
{
    public JobBoardModule(IConfiguration configuration)
    {
        var services = new ServiceCollection();

        ConfigureServices(configuration, services);

        JobBoardModuleCompositionRoot.SetServiceProvider(services.BuildServiceProvider());
    }

    private void ConfigureServices(IConfiguration configuration, ServiceCollection services)
    {
        services
            .AddSingleton<IDateTimeProvider, SystemDateTime>()
            .AddMediator(ModuleApplicationAssembly,
                             typeof(LoggingMiddleware<,>),
                             typeof(ValidationMiddleware<,>),
                             typeof(UnitOfWorkMiddleware<,>))
            .AddStorageApiClient(configuration["StorageApi:Url"])
            .AddDomainEventDispatcher()
            .AddUserDataProvider();

        services
            .AddValidatorsFromAssembly(ModuleApplicationAssembly)
            .AddJobBoardContext(configuration["MongoDb:ConnectionString"]!, configuration["MongoDb:DatabaseName"]!)
            .AddPersistence();

    }

    public Assembly ModuleApplicationAssembly => typeof(JobBoardEndpointsConfiguration).Assembly;

    public void ConfigureModule(WebApplication app)
    {
        JobBoardEndpointsConfiguration.MapEndpoints(app);
    }

    protected override Func<IServiceScope> ScopeProvider => JobBoardModuleCompositionRoot.CreateScope;
}