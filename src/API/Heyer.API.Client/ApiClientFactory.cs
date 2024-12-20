using RestEase;

namespace Heyer.API.Client;

public static class ApiClientFactory
{
    public static IApiClient Create(HttpClient client) => RestClient.For<IApiClient>(client);

    public static IApiClient Create(string baseUrl, TimeSpan? timeout = null)
    {
        var client = new HttpClient { BaseAddress = new Uri(baseUrl), Timeout = timeout ?? TimeSpan.FromSeconds(30) };

        return RestClient.For<IApiClient>(client);
    }
}