using System.Reflection;
using Heyer.BuildingBlocks.Infrastructure.Modules;
using Heyer.Modules.Hiring.Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Heyer.Modules.Hiring.Infrastructure;

public class HiringModule : IModule
{
    public Assembly ModuleApplicationAssembly => typeof(HiringEndpointsConfiguration).Assembly;

    public void ConfigureDependencyInjection(IServiceCollection services)
    {
    }

    public void ConfigureModule(WebApplication app)
    {
    }
}