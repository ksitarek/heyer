using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Heyer.BuildingBlocks.Infrastructure.Modules;

public interface IModule
{
    void ConfigureDependencyInjection(IServiceCollection services);
    void ConfigureModule(WebApplication app);
}