using RestEase;

namespace Heyer.Storage.API.Client;

public static class StorageApiClientFactory
{
    public static IStorageApiClient Create(HttpClient client)
    {
        return RestClient.For<IStorageApiClient>(client);
    }
    
    public static IStorageApiClient Create(string baseUrl)
    {
        return RestClient.For<IStorageApiClient>(baseUrl);
    }
}