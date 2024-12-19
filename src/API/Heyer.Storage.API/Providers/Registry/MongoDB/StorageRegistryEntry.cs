using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Heyer.Storage.API.Providers.Registry.MongoDB;

public class StorageRegistryEntry : IFileProperties
{
    public string ContentType { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public string FileName { get; set; } = default!;

    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public string Key { get; set; } = default!;

    public bool Preserve { get; set; } = false;
    public long Size { get; set; }
}