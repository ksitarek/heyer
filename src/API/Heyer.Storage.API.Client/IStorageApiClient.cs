using Heyer.Storage.API.Client.PublishedLanguage;
using RestEase;

namespace Heyer.Storage.API.Client;

public interface IStorageApiClient : IDisposable
{
    [Delete("/delete/{key}")]
    Task Delete([Path] string key);

    [Get("/download/{key}")]
    Task<Response<DownloadResponse>> Download([Path] string key);

    [Get("/csrf")]
    Task<string> GetCsrfToken();

    [Get("/health")]
    [AllowAnyStatusCode]
    Task<Response<dynamic>> HealthCheck();

    [Post("/preserve/{key}")]
    Task Preserve([Path] string key);

    [Post("/store")]
    Task<StoreResult> Store([Header("X-XSRF-TOKEN")] string csrf, [Body] MultipartFormDataContent file);
}