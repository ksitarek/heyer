using System.Text.Json;
using RestEase;

namespace Heyer.API.Client;

public static class ApiClientFactory
{
    public static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = null
    };

    private static readonly Serializer Serializer = new(SerializerOptions);
    private static readonly Deserializer Deserializer = new(SerializerOptions);

    public static IApiClient Create(HttpClient client)
    {
        return new RestClient(client)
        {
            RequestBodySerializer = Serializer,
            ResponseDeserializer = Deserializer
        }.For<IApiClient>();
    }

    public static IApiClient Create(string baseUrl, TimeSpan? timeout = null)
    {
        var client = new HttpClient { BaseAddress = new Uri(baseUrl), Timeout = timeout ?? TimeSpan.FromSeconds(30) };

        return new RestClient(client)
        {
            RequestBodySerializer = Serializer,
            ResponseDeserializer = Deserializer
        }.For<IApiClient>();
    }
}