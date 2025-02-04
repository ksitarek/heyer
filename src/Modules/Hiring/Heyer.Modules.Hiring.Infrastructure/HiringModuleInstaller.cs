using FluentValidation;
using Hangfire;
using Heyer.BuildingBlocks.Application.Authorization;
using Heyer.BuildingBlocks.Infrastructure;
using Heyer.BuildingBlocks.Infrastructure.Integration;
using Heyer.BuildingBlocks.Infrastructure.Mediator;
using Heyer.BuildingBlocks.Infrastructure.Mediator.Middleware;
using Heyer.BuildingBlocks.Infrastructure.Messaging;
using Heyer.BuildingBlocks.Infrastructure.Modules;
using Heyer.Modules.Hiring.Application;
using Heyer.Modules.Hiring.Infrastructure.Configuration;
using Heyer.Modules.Hiring.Infrastructure.Integration;
using Heyer.Storage.API.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Heyer.Modules.Hiring.Infrastructure;

public class HiringModuleInstaller : ModuleInstaller, IHiringModuleInstaller
{
    protected override Func<IServiceScope> ScopeProvider { get; }
        = HiringModuleCompositionRoot.CreateScope;

    public override void ConfigureEventBusSubscriptions(IEventBus eventBus)
    {
    }

    public override void ConfigureServiceProvider()
    {
        EnsureEventBusIsSet();
        EnsureConfigurationIsSet();

        var assembly = typeof(IHiringModule).Assembly;

        var services = new ServiceCollection();

        services
            .AddSingleton(EventBus!)
            .AddSingleton<IDateTimeProvider, SystemDateTime>()
            .AddMediator(assembly,
                         typeof(LoggingMiddleware<,>),
                         typeof(ValidationMiddleware<,>),
                         typeof(UnitOfWorkMiddleware<,>))
            .AddStorageApiClient(Configuration!["StorageApi:Url"])
            .AddDomainEventDispatcher(assembly)
            .AddUserDataProvider()
            .AddValidatorsFromAssembly(assembly)
            .AddHiringDbContext(Configuration!.GetSection("Companies"))
            .AddPersistence();

        HiringModuleCompositionRoot.SetServiceProvider(services.BuildServiceProvider());
    }

    public override void RegisterInGlobalContainer(IServiceCollection globalServices) =>
        globalServices.AddSingleton<IHiringModule, HiringModule>(_ => new HiringModule(ScopeProvider));

    protected override void ConfigureEndpoints(WebApplication app) =>
        HiringEndpointsConfiguration.MapEndpoints(app);

    protected override void ConfigureScheduler(IRecurringJobManager recurringJobManager)
    {
        recurringJobManager.AddOrUpdate<HiringInboxProcessingJob>(
            $"{nameof(HiringModuleInstaller)}_InboxProcessing",
            job => job.Handle(),
            "* * * * * *");
        recurringJobManager.AddOrUpdate<HiringOutboxProcessingJob>(
            $"{nameof(HiringModuleInstaller)}_OutboxProcessing",
            job => job.Handle(),
            "* * * * * *");
    }
}