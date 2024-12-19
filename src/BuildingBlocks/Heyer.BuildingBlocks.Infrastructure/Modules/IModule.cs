using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Heyer.BuildingBlocks.Infrastructure.Modules;

public interface IModule
{
    Assembly ModuleApplicationAssembly { get; }

    void ConfigureDependencyInjection(IServiceCollection services);
    void ConfigureModule(WebApplication app);
}