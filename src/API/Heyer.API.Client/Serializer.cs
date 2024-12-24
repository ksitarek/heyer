using System.Net.Http.Headers;
using System.Text.Json;
using RestEase;

namespace Heyer.API.Client;

internal class Serializer : RequestBodySerializer
{
    private readonly JsonSerializerOptions _options;

    public Serializer(JsonSerializerOptions? options = null) => this._options = options ?? JsonSerializerOptions.Default;

    public override HttpContent? SerializeBody<T>(T body, RequestBodySerializerInfo info)
    {
        if (body == null)
        {
            return null;
        }

        var content = new StringContent(JsonSerializer.Serialize(body, _options));

        const string contentType = "application/json";
        if (content.Headers.ContentType == null)
        {
            content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
        }
        else
        {
            content.Headers.ContentType.MediaType = contentType;
        }

        return content;
    }
}