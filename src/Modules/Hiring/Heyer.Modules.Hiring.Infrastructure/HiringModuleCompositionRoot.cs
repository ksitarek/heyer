using Microsoft.Extensions.DependencyInjection;

namespace Heyer.Modules.Hiring.Infrastructure;

internal static class HiringModuleCompositionRoot
{
    private static IServiceProvider _serviceProvider = null!;

    public static IServiceScope CreateScope() => _serviceProvider.CreateScope();

    internal static void SetServiceProvider(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;
}