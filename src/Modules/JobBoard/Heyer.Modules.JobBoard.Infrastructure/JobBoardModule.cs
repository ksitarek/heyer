using Heyer.BuildingBlocks.Infrastructure.Modules;
using Heyer.Modules.JobBoard.Application;
using Microsoft.Extensions.DependencyInjection;

namespace Heyer.Modules.JobBoard.Infrastructure;

public class JobBoardModule : ModuleRunner, IJobBoardModule
{
    public JobBoardModule(Func<IServiceScope> scopeProvider) : base(scopeProvider)
    {
    }
}