using System.Text.Json;
using RestEase;

namespace Heyer.API.Client;

internal class Deserializer : ResponseDeserializer
{
    private readonly JsonSerializerOptions options;

    public Deserializer(JsonSerializerOptions? options = null)
    {
        this.options = options ?? JsonSerializerOptions.Default;
    }

    public override T Deserialize<T>(string? content, HttpResponseMessage response, ResponseDeserializerInfo info)
    {
        return JsonSerializer.Deserialize<T>(content!, options)!;
    }
}