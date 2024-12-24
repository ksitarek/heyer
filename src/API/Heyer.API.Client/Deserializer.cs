using System.Text.Json;
using RestEase;

namespace Heyer.API.Client;

internal class Deserializer : ResponseDeserializer
{
    private readonly JsonSerializerOptions _options;

    public Deserializer(JsonSerializerOptions? options = null) =>
        _options = options ?? JsonSerializerOptions.Default;

    public override T Deserialize<T>(string? content, HttpResponseMessage response, ResponseDeserializerInfo info) =>
        JsonSerializer.Deserialize<T>(content!, _options)!;
}