using System.Text.Json;
using System.Text.Json.Serialization;
using RestEase;

namespace Heyer.API.Client;

public static class ApiClientFactory
{
    public static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = null,
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.Never,
        Converters = { new JsonStringEnumConverter() }
    };

    private static readonly Deserializer _deserializer = new(SerializerOptions);

    private static readonly Serializer _serializer = new(SerializerOptions);

    public static IApiClient Create(HttpClient client) =>
        new RestClient(client) { RequestBodySerializer = _serializer, ResponseDeserializer = _deserializer }
            .For<IApiClient>();

    public static IApiClient Create(string baseUrl, TimeSpan? timeout = null)
    {
        var client = new HttpClient { BaseAddress = new Uri(baseUrl), Timeout = timeout ?? TimeSpan.FromSeconds(30) };

        return new RestClient(client) { RequestBodySerializer = _serializer, ResponseDeserializer = _deserializer }
            .For<IApiClient>();
    }
}