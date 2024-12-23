using System.Text.Json;
using System.Text.Json.Serialization;
using RestEase;

namespace Heyer.API.Client;

public static class ApiClientFactory
{
    public static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = null,
        DefaultIgnoreCondition = JsonIgnoreCondition.Never
    };

    private static readonly Deserializer Deserializer = new(SerializerOptions);

    private static readonly Serializer Serializer = new(SerializerOptions);

    public static IApiClient Create(HttpClient client) =>
        new RestClient(client) { RequestBodySerializer = Serializer, ResponseDeserializer = Deserializer }
            .For<IApiClient>();

    public static IApiClient Create(string baseUrl, TimeSpan? timeout = null)
    {
        var client = new HttpClient { BaseAddress = new Uri(baseUrl), Timeout = timeout ?? TimeSpan.FromSeconds(30) };

        return new RestClient(client) { RequestBodySerializer = Serializer, ResponseDeserializer = Deserializer }
            .For<IApiClient>();
    }
}