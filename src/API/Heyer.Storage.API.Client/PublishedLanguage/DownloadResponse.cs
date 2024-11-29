namespace Heyer.Storage.API.Client.PublishedLanguage;

public record DownloadResponse(string FileName, string ContentType, Stream FileContent);