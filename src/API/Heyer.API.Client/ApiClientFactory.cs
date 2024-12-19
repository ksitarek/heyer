using RestEase;

namespace Heyer.API.Client;

public static class ApiClientFactory
{
    public static IApiClient Create(HttpClient client) => RestClient.For<IApiClient>(client);

    public static IApiClient Create(string baseUrl) => RestClient.For<IApiClient>(baseUrl);
}