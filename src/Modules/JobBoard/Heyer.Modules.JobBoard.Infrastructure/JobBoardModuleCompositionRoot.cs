using Microsoft.Extensions.DependencyInjection;

namespace Heyer.Modules.JobBoard.Infrastructure;

internal static class JobBoardModuleCompositionRoot
{
    private static IServiceProvider _serviceProvider = null!;

    internal static void SetServiceProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public static IServiceScope CreateScope() => _serviceProvider.CreateScope();
}