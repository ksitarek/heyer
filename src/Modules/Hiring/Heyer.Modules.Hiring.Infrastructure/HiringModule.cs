using Heyer.BuildingBlocks.Infrastructure.Modules;
using Heyer.Modules.Hiring.Application;
using Microsoft.Extensions.DependencyInjection;

namespace Heyer.Modules.Hiring.Infrastructure;

internal class HiringModule : ModuleRunner, IHiringModule
{
    public HiringModule(Func<IServiceScope> scopeProvider) : base(scopeProvider)
    {
    }
}