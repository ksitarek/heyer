using RestEase;

namespace Heyer.API.Client;

public static class ApiClientFactory
{
    public static IApiClient Create(HttpClient client)
    {
        return RestClient.For<IApiClient>(client);
    }
    
    public static IApiClient Create(string baseUrl)
    {
        return RestClient.For<IApiClient>(baseUrl);
    }
}