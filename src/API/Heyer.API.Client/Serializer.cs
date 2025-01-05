using System.Net.Http.Headers;
using Heyer.BuildingBlocks.Json;
using RestEase;

namespace Heyer.API.Client;

internal class Serializer : RequestBodySerializer
{
    public override HttpContent? SerializeBody<T>(T body, RequestBodySerializerInfo info)
    {
        if (body == null)
        {
            return null;
        }

        var content = new StringContent(body.Serialize());

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