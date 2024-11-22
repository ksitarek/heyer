using System.Diagnostics.CodeAnalysis;

namespace Heyer.Storage.API.Tests.IntegrationTests;

[SuppressMessage("ReSharper", "InconsistentNaming")]
internal static class Config
{
    public const string StorageStrategy_Type = "StorageStrategy:Type";
    public const string StorageStrategy_FilesystemStorage_RootPath = "StorageStrategy:FilesystemStorage:RootPath";
}