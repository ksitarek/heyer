using Heyer.BuildingBlocks.Infrastructure.Integration;
using Heyer.BuildingBlocks.Infrastructure.Integration.Persistence;
using Heyer.BuildingBlocks.Infrastructure.Integration.Processing;

namespace Heyer.Modules.JobBoard.Infrastructure.Integration;

internal class JobBoardIntegrationHandler<T> : GenericEventHandler<T> where T : IntegrationEvent
{
    public JobBoardIntegrationHandler(IInboxStore inboxStore) : base(inboxStore)
    {
    }
}