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
        services.AddSingleton<IDateTimeProvider, SystemDateTime>();

        services.AddMediator(ModuleApplicationAssembly,
                             typeof(LoggingMiddleware<,>),
                             typeof(ValidationMiddleware<,>),
                             typeof(UnitOfWorkMiddleware<,>));

        services.AddStorageApiClient(configuration["StorageApi:Url"]);

        services.AddValidatorsFromAssembly(ModuleApplicationAssembly);

        var client = new MongoClient(configuration["MongoDb:ConnectionString"]!);
        var db = client.GetDatabase(configuration["MongoDb:DatabaseName"]!);

        services.AddSingleton(db);

        services.AddDbContext<JobBoardContext>(o =>
                                                   o.UseMongoDB(db.Client, db.DatabaseNamespace.DatabaseName));

        services.AddScoped<DbContext>(sp => sp.GetRequiredService<JobBoardContext>());

        services.AddScoped<IPublishedJobOffersRepository, PublishedJobOffersRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddDomainEventDispatcher();
        services.AddUserDataProvider();
    }

    public Assembly ModuleApplicationAssembly => typeof(JobBoardEndpointsConfiguration).Assembly;

    public void ConfigureModule(WebApplication app)
    {
        JobBoardEndpointsConfiguration.MapEndpoints(app);
    }

    protected override Func<IServiceScope> ScopeProvider => JobBoardModuleCompositionRoot.CreateScope;
}