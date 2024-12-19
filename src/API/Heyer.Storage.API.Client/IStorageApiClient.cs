using Heyer.Storage.API.Client.PublishedLanguage;
using RestEase;

namespace Heyer.Storage.API.Client;

public interface IStorageApiClient
{
    [Delete("/delete/{key}")]
    Task Delete([Path] string key);

    [Get("/download/{key}")]
    Task<Response<DownloadResponse>> Download([Path] string key);

    [Get("/csrf")]
    Task<string> GetCsrfToken();

    [Post("/preserve/{key}")]
    Task Preserve([Path] string key);

    [Post("/store")]
    Task<StoreResult> Store([Header("RequestVerificationToken")] string csrf, [Body] MultipartFormDataContent file);
}