using Microsoft.AspNetCore.Builder;

namespace Heyer.BuildingBlocks.Infrastructure.Modules;

public interface IModuleInstaller
{
    void ConfigureModule(WebApplication app);
}