using Microsoft.EntityFrameworkCore;

namespace Heyer.BuildingBlocks.Infrastructure.Integration.Persistence;

public interface IInboxContext
{
    DbSet<InboxMessage> InboxMessages { get; init; }
}