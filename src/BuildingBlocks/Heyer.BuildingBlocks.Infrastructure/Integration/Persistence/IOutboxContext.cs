using Microsoft.EntityFrameworkCore;

namespace Heyer.BuildingBlocks.Infrastructure.Integration.Persistence;

public interface IOutboxContext
{
    DbSet<OutboxMessage> OutboxMessages { get; init; }
}