using Heyer.BuildingBlocks.Json;
using RestEase;

namespace Heyer.API.Client;

internal class Deserializer : ResponseDeserializer
{
    public override T Deserialize<T>(string? content, HttpResponseMessage response, ResponseDeserializerInfo info) =>
        content!.Deserialize<T>()!;
}