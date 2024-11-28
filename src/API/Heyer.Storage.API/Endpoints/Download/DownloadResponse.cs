namespace Heyer.Storage.API.Endpoints.Download;

public record DownloadResponse(string FileName, string ContentType, Stream FileContent);