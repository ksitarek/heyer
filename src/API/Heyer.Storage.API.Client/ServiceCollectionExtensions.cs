using Microsoft.Extensions.DependencyInjection;

namespace Heyer.Storage.API.Client;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddStorageApiClient(this IServiceCollection services, string? baseUrl)
    {
        ArgumentNullException.ThrowIfNull(baseUrl, nameof(baseUrl));

        services.AddSingleton<IStorageApiClient>(provider => StorageApiClientFactory.Create(baseUrl));
        return services;
    }
}