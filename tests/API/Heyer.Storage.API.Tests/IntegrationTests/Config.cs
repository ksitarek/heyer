using System.Diagnostics.CodeAnalysis;

namespace Heyer.Storage.API.Tests.IntegrationTests;

[SuppressMessage("ReSharper", "InconsistentNaming")]
internal static class Config
{
    public const string StorageStrategy_Type = "StorageStrategy:Type";
    public const string StorageStrategy_FilesystemStorage_RootPath = "StorageStrategy:FilesystemStorage:RootPath";
    
    public const string RegistryStrategy_Type = "RegistryStrategy:Type";
    public const string RegistryStrategy_MongoDbRegistry_ConnectionString = "RegistryStrategy:MongoDbRegistry:ConnectionString";
    public const string RegistryStrategy_MongoDbRegistry_DatabaseName = "RegistryStrategy:MongoDbRegistry:DatabaseName";
    public const string RegistryStrategy_MongoDbRegistry_CollectionName = "RegistryStrategy:MongoDbRegistry:CollectionName";
}