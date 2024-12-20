using RestEase;

namespace Heyer.Storage.API.Client;

public static class StorageApiClientFactory
{
    public static IStorageApiClient Create(HttpClient client) => RestClient.For<IStorageApiClient>(client);

    public static IStorageApiClient Create(string baseUrl, TimeSpan? timeout = null)
    {
        var client = new HttpClient { BaseAddress = new Uri(baseUrl), Timeout = timeout ?? TimeSpan.FromSeconds(30) };

        return RestClient.For<IStorageApiClient>(client);
    }
}