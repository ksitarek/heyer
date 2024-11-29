using Heyer.Storage.API.Client.PublishedLanguage;
using RestEase;

namespace Heyer.Storage.API.Client;

public interface IStorageApiClient
{
    [Get("/csrf")]
    Task<string> GetCsrfToken();
    
    [Post("/store")]
    Task<StoreResult> Store([Header("RequestVerificationToken")] string csrf, [Body] MultipartFormDataContent file);
    
    [Get("/download/{key}")]
    Task<Response<DownloadResponse>> Download([Path]string key);
    
    [Delete("/delete/{key}")]
    Task Delete([Path]string key);
    
    [Post("/preserve/{key}")]
    Task Preserve([Path]string key);
}