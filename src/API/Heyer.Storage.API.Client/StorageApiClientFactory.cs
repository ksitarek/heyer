using RestEase;

namespace Heyer.Storage.API.Client;

public static class StorageApiClientFactory
{
    public static IStorageApiClient Create(HttpClient client) => RestClient.For<IStorageApiClient>(client);

    public static IStorageApiClient Create(string baseUrl) => RestClient.For<IStorageApiClient>(baseUrl);
}