using FluentValidation;
using Hangfire;
using Heyer.BuildingBlocks.Application.Authorization;
using Heyer.BuildingBlocks.Infrastructure;
using Heyer.BuildingBlocks.Infrastructure.HealthChecks;
using Heyer.BuildingBlocks.Infrastructure.Integration;
using Heyer.BuildingBlocks.Infrastructure.Integration.Processing;
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
using Microsoft.Extensions.DependencyInjection;

namespace Heyer.Modules.JobBoard.Infrastructure;

public class JobBoardModuleInstaller : ModuleInstaller, IJobBoardModuleInstaller
{
    protected override Func<IServiceScope> ScopeProvider { get; }
        = JobBoardModuleCompositionRoot.CreateScope;

    public override void ConfigureEventBusSubscriptions(IEventBus eventBus)
    {
        eventBus.Subscribe(
            new GenericEventHandler<JobOfferPublishedIntegrationEvent>(JobBoardModuleCompositionRoot.ServiceProvider));
        eventBus.Subscribe(
            new GenericEventHandler<JobOfferTakenDownIntegrationEvent>(JobBoardModuleCompositionRoot.ServiceProvider));
    }

    public override void ConfigureServiceProvider()
    {
        EnsureEventBusIsSet();
        EnsureConfigurationIsSet();

        var assembly = typeof(IJobBoardModule).Assembly;

        var services = new ServiceCollection();

        services
            .AddSingleton(EventBus!)
            .AddSingleton<IJobBoardModule, JobBoardModule>()
            .AddSingleton<IDateTimeProvider, SystemDateTime>()
            .AddScoped<JobBoardOutboxProcessingJobRun>()
            .AddTransient<IHealthCheck, JobBoardDatabaseHealthcheck>()
            .AddMediator(assembly,
                         typeof(LoggingMiddleware<,>),
                         typeof(ValidationMiddleware<,>),
                         typeof(UnitOfWorkMiddleware<,>))
            .AddStorageApiClient(Configuration!["StorageApi:Url"])
            .AddDomainEventDispatcher(assembly)
            .AddUserDataProvider()
            .AddValidatorsFromAssembly(assembly)
            .AddJobBoardContext(Configuration!["Npgsql:ConnectionString"]!)
            .AddPersistence();

        JobBoardModuleCompositionRoot.SetServiceProvider(services.BuildServiceProvider());
    }

    public override void RegisterInGlobalContainer(IServiceCollection globalServices) =>
        globalServices.AddSingleton<IJobBoardModule, JobBoardModule>(_ => new JobBoardModule(ScopeProvider));

    protected override void ConfigureEndpoints(WebApplication app) => JobBoardEndpointsConfiguration.MapEndpoints(app);

    protected override void ConfigureScheduler(IRecurringJobManager recurringJobManager)
    {
        recurringJobManager.AddOrUpdate<JobBoardInboxProcessingJob>(
            $"{nameof(JobBoardModuleInstaller)}_InboxProcessing",
            job => job.Handle(),
            "* * * * * *");

        recurringJobManager.AddOrUpdate(
            $"{nameof(JobBoardModuleInstaller)}_OutboxProcessing",
            () => JobBoardOutboxProcessingJob.RunAsync(),
            "* * * * * *");
    }
}