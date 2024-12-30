namespace Heyer.BuildingBlocks.Infrastructure.Integration.Persistence;

public class OutboxMessage
{
    public DateTime CreatedAt { get; set; }
    public required string Data { get; set; }
    public Guid Id { get; set; }

    public DateTime? ProcessedAt { get; set; }
    public required string Type { get; set; }
}