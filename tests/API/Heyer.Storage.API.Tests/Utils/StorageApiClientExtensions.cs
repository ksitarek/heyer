using Heyer.Storage.API.Client;
using Heyer.Storage.API.Client.PublishedLanguage;

namespace Heyer.Storage.API.Tests.Utils;

public static class StorageApiClientExtensions
{
    public static async Task<StoreResult> Store(this IStorageApiClient client, string filePath)
    {
        await using var file = File.OpenRead(filePath);
        using var streamContent = new StreamContent(file);
        using var formData = new MultipartFormDataContent();
        formData.Add(streamContent, "file", Path.GetFileName(filePath));

        var csrfToken = await client.GetCsrfToken();

        return await client.Store(csrfToken, formData);
    }
}