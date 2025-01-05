using Heyer.BuildingBlocks.Json;

namespace Heyer.Storage.API.Tests.Utils;

internal static class HttpClientExtensions
{
    public static async Task<string> GetCsrfToken(this HttpClient client)
    {
        var response = await client.GetAsync("/csrf");
        return await response.Content.ReadAsStringAsync();
    }

    public static async Task<TResult?> ReadContentAs<TResult>(this HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        return content.Deserialize<TResult>();
    }
}