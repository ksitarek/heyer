using Microsoft.Extensions.DependencyInjection;

namespace Heyer.Modules.JobBoard.Infrastructure;

internal static class JobBoardModuleCompositionRoot
{
    private static IServiceProvider? _serviceProvider;

    public static IServiceProvider ServiceProvider
    {
        get
        {
            EnsureServiceProviderIsSet();

            return _serviceProvider!;
        }
    }

    public static IServiceScope CreateScope()
    {
        EnsureServiceProviderIsSet();

        return _serviceProvider!.CreateScope();
    }

    internal static void SetServiceProvider(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    private static void EnsureServiceProviderIsSet()
    {
        if (_serviceProvider == null)
        {
            throw new InvalidOperationException("Service provider is not set");
        }
    }
}