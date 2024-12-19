namespace Heyer.BuildingBlocks.Tests;

public interface IApplicationFactory<TApiClient> : IAsyncDisposable
{
    TApiClient CreateApiClient();
    TApiClient CreateAuthorizedApiClient(params string[] permissions);
    TService GetRequiredService<TService>() where TService : class;
}