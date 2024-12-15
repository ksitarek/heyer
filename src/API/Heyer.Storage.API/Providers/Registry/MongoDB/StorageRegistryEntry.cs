using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Heyer.Storage.API.Providers.Registry.MongoDB;

public class StorageRegistryEntry : IFileProperties
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public string Key { get; set; } = default!;
    public string FileName { get; set; } = default!;
    public string ContentType { get; set; } = default!;
    public long Size { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool Preserve { get; set; } = false;
}