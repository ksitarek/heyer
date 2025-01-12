using System.Reflection;
using FluentValidation;
using Hangfire;
using Heyer.BuildingBlocks.Application.Authorization;
using Heyer.BuildingBlocks.Application.Notifications;
using Heyer.BuildingBlocks.Infrastructure;
using Heyer.BuildingBlocks.Infrastructure.HealthChecks;
using Heyer.BuildingBlocks.Infrastructure.Integration;
using Heyer.BuildingBlocks.Infrastructure.Integration.Persistence;
using Heyer.BuildingBlocks.Infrastructure.Mediator;
using Heyer.BuildingBlocks.Infrastructure.Mediator.Middleware;
using Heyer.BuildingBlocks.Infrastructure.Messaging;
using Heyer.BuildingBlocks.Infrastructure.Modules;
using Heyer.Modules.Hiring.PublishedLanguage.IntegrationEvents;
using Heyer.Modules.JobBoard.Application;
using Heyer.Modules.JobBoard.Infrastructure.Configuration;
using Heyer.Modules.JobBoard.Infrastructure.HealthChecks;
using Heyer.Modules.JobBoard.Infrastructure.Integration;
using Heyer.Storage.API.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Heyer.Modules.JobBoard.Infrastructure;

public class JobBoardModule : ModuleRunner, IJobBoardModule
{
    private readonly IEventBus _eventBus;
    private readonly ServiceProvider _serviceProvider;

    public JobBoardModule(IConfiguration configuration, IEventBus eventBus)
    {
        _eventBus = eventBus;

        var services = new ServiceCollection();

        ConfigureServices(configuration, services);

        _serviceProvider = services.BuildServiceProvider();

        JobBoardModuleCompositionRoot.SetServiceProvider(_serviceProvider);
    }

    public Assembly ModuleApplicationAssembly => typeof(JobBoardEndpointsConfiguration).Assembly;

    public override Func<IServiceScope> ScopeProvider => JobBoardModuleCompositionRoot.CreateScope;

    public void ConfigureModule(WebApplication app)
    {
        JobBoardEndpointsConfiguration.MapEndpoints(app);

        var inboxStore = _serviceProvider.GetRequiredService<IInboxStore>();

        _eventBus.Subscribe(new JobBoardIntegrationHandler<JobOfferPublishedIntegrationEvent>(inboxStore));

        var recurringJobManager = app.Services.GetRequiredService<IRecurringJobManager>();
        recurringJobManager.AddOrUpdate<JobBoardInboxProcessingJob>(
            $"{nameof(JobBoardModule)}_InboxProcessing",
            job => job.Handle(),
            "* * * * * *");
        recurringJobManager.AddOrUpdate(
            $"{nameof(JobBoardModule)}_OutboxProcessing",
            () => JobBoardOutboxProcessingJob.RunAsync(),
            "* * * * * *");
    }

    private void ConfigureServices(IConfiguration configuration, ServiceCollection services)
    {
        var domainEventNotificationsRegistry = new DomainEventNotificationsRegistry();

        services
            .AddSingleton(_eventBus)
            .AddSingleton<IDateTimeProvider, SystemDateTime>()
            .AddScoped<JobBoardOutboxProcessingJobRun>()
            .AddTransient<IHealthCheck, JobBoardDatabaseHealthcheck>();

        services
            .AddMediator(ModuleApplicationAssembly,
                         typeof(LoggingMiddleware<,>),
                         typeof(ValidationMiddleware<,>),
                         typeof(UnitOfWorkMiddleware<,>))
            .AddStorageApiClient(configuration["StorageApi:Url"])
            .AddDomainEventDispatcher(domainEventNotificationsRegistry)
            .AddUserDataProvider()
            .AddValidatorsFromAssembly(ModuleApplicationAssembly)
            .AddJobBoardContext(configuration["Npgsql:ConnectionString"]!)
            .AddPersistence();
    }
}