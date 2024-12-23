using System.Reflection;
using FluentValidation;
using Heyer.BuildingBlocks.Application.Authorization;
using Heyer.BuildingBlocks.Infrastructure;
using Heyer.BuildingBlocks.Infrastructure.Mediator;
using Heyer.BuildingBlocks.Infrastructure.Mediator.Middleware;
using Heyer.BuildingBlocks.Infrastructure.Messaging;
using Heyer.BuildingBlocks.Infrastructure.Modules;
using Heyer.Modules.Hiring.Application;
using Heyer.Modules.Hiring.Infrastructure.Configuration;
using Heyer.Storage.API.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Heyer.Modules.Hiring.Infrastructure;

public class HiringModule : ModuleRunner, IHiringModule, IModuleInstaller
{
    public HiringModule(IConfiguration configuration)
    {
        var services = new ServiceCollection();

        ConfigureServices(configuration, services);

        HiringModuleCompositionRoot.SetServiceProvider(services.BuildServiceProvider());
    }

    public Assembly ModuleApplicationAssembly => typeof(HiringEndpointsConfiguration).Assembly;

    public override Func<IServiceScope> ScopeProvider => HiringModuleCompositionRoot.CreateScope;

    public void ConfigureModule(WebApplication app) => HiringEndpointsConfiguration.MapEndpoints(app);

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
            .AddHiringDbContext(configuration.GetSection("Companies"))
            .AddPersistence();
    }
}