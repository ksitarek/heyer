using System.Diagnostics.CodeAnalysis;

namespace Heyer.Storage.API.Tests.IntegrationTests;

[SuppressMessage("ReSharper", "InconsistentNaming")]
internal static class Config
{
    public const string Jwt_Secret = "Jwt:Secret";
    public const string Jwt_ValidateAudience = "Jwt:ValidateAudience";

    public const string Jwt_ValidateIssuer = "Jwt:ValidateIssuer";
    public const string Jwt_ValidateLifetime = "Jwt:ValidateLifetime";
    public const string Jwt_ValidAudience = "Jwt:ValidAudience";
    public const string Jwt_ValidIssuer = "Jwt:ValidIssuer";

    public const string RegistryStrategy_MongoDbRegistry_CollectionName =
        "RegistryStrategy:MongoDbRegistry:CollectionName";

    public const string RegistryStrategy_MongoDbRegistry_ConnectionString =
        "RegistryStrategy:MongoDbRegistry:ConnectionString";

    public const string RegistryStrategy_MongoDbRegistry_DatabaseName = "RegistryStrategy:MongoDbRegistry:DatabaseName";

    public const string RegistryStrategy_Type = "RegistryStrategy:Type";
    public const string StorageStrategy_FilesystemStorage_RootPath = "StorageStrategy:FilesystemStorage:RootPath";
    public const string StorageStrategy_Type = "StorageStrategy:Type";
}