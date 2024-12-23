using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Heyer.BuildingBlocks.Infrastructure.Modules;

public interface IModuleInstaller
{
    Assembly ModuleApplicationAssembly { get; }

    void ConfigureModule(WebApplication app);
}