using System.Reflection;
using FluentValidation;
using Hangfire;
using Heyer.BuildingBlocks.Application.Authorization;
using Heyer.BuildingBlocks.Application.Notifications;
using Heyer.BuildingBlocks.Infrastructure;
using Heyer.BuildingBlocks.Infrastructure.Integration;
using Heyer.BuildingBlocks.Infrastructure.Mediator;
using Heyer.BuildingBlocks.Infrastructure.Mediator.Middleware;
using Heyer.BuildingBlocks.Infrastructure.Messaging;
using Heyer.BuildingBlocks.Infrastructure.Modules;
using Heyer.Modules.Hiring.Application;
using Heyer.Modules.Hiring.Application.JobOffers.Publish;
using Heyer.Modules.Hiring.Domain.JobOffers.Events;
using Heyer.Modules.Hiring.Infrastructure.Configuration;
using Heyer.Modules.Hiring.Infrastructure.Integration;
using Heyer.Storage.API.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Heyer.Modules.Hiring.Infrastructure;

public class HiringModule : ModuleRunner, IHiringModule, IModuleInstaller
{
    private readonly IConfiguration _configuration;
    private readonly IEventBus _eventBus;

    public HiringModule(IConfiguration configuration, IEventBus eventBus)
    {
        _configuration = configuration;
        _eventBus = eventBus;

        var services = new ServiceCollection();

        ConfigureServices(_configuration, services);

        HiringModuleCompositionRoot.SetServiceProvider(services.BuildServiceProvider());
    }

    public Assembly ModuleApplicationAssembly => typeof(HiringEndpointsConfiguration).Assembly;

    public override Func<IServiceScope> ScopeProvider => HiringModuleCompositionRoot.CreateScope;

    public void ConfigureModule(WebApplication app)
    {
        HiringEndpointsConfiguration.MapEndpoints(app);

        var recurringJobManager = app.Services.GetRequiredService<IRecurringJobManager>();
        recurringJobManager.AddOrUpdate<HiringInboxProcessingJob>(
            $"{nameof(HiringModule)}_InboxProcessing",
            job => job.Handle(),
            "* * * * * *");
        recurringJobManager.AddOrUpdate<HiringOutboxProcessingJob>(
            $"{nameof(HiringModule)}_OutboxProcessing",
            job => job.Handle(),
            "* * * * * *");
    }

    private void ConfigureServices(IConfiguration configuration, ServiceCollection services)
    {
        services.AddSingleton(_eventBus);

        var notificationsRegistry = new DomainEventNotificationsRegistry();
        notificationsRegistry.Add<JobOfferPublishedNotification, JobOfferPublished>();

        services
            .AddSingleton<IDateTimeProvider, SystemDateTime>()
            .AddMediator(ModuleApplicationAssembly,
                         typeof(LoggingMiddleware<,>),
                         typeof(ValidationMiddleware<,>),
                         typeof(UnitOfWorkMiddleware<,>))
            .AddStorageApiClient(configuration["StorageApi:Url"])
            .AddDomainEventDispatcher(notificationsRegistry)
            .AddUserDataProvider();

        services
            .AddValidatorsFromAssembly(ModuleApplicationAssembly)
            .AddHiringDbContext(configuration.GetSection("Companies"),
                                configuration.GetSection("HiringModule:InboxOutbox"))
            .AddPersistence();
    }
}