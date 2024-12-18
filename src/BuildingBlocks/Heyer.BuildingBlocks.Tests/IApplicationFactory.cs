namespace Heyer.BuildingBlocks.Tests;

public interface IApplicationFactory<TApiClient> : IAsyncDisposable
{
    TService GetRequiredService<TService>() where TService : class;
    TApiClient CreateApiClient();
    TApiClient CreateAuthorizedApiClient();
}