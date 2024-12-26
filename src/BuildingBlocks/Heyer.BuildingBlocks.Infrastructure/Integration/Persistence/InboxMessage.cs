using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Heyer.BuildingBlocks.Infrastructure.Integration.Persistence;

public class InboxMessage
{
    public DateTime CreatedAt { get; set; }
    public required string Data { get; set; }

    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; set; }

    public DateTime? ProcessedAt { get; set; }
    public required string Type { get; set; }
}