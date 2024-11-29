using Heyer.Storage.API.Client;

namespace Heyer.Storage.API.Tests.IntegrationTests;

internal interface IApplicationFactory : IAsyncDisposable
{
    TService GetRequiredService<TService>() where TService : class;
    IStorageApiClient CreateApiClient();
    IStorageApiClient CreateAuthorizedApiClient();
}