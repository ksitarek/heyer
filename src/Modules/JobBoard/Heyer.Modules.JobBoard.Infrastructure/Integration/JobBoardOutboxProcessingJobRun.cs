using Heyer.BuildingBlocks.Application.Authorization;
using Heyer.BuildingBlocks.Infrastructure.Integration.Persistence;
using Heyer.BuildingBlocks.Infrastructure.Integration.Processing;
using MediatR;

namespace Heyer.Modules.JobBoard.Infrastructure.Integration;

internal class JobBoardOutboxProcessingJobRun : GenericOutboxProcessingJob
{
    public JobBoardOutboxProcessingJobRun(IMediator mediator,
                                          IUserDataProvider userDataProvider,
                                          IOutboxStore outboxStore)
        : base(mediator, userDataProvider, outboxStore)
    {
    }
}