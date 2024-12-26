using Heyer.BuildingBlocks.Application.Authorization;
using Heyer.BuildingBlocks.Infrastructure.Integration.Persistence;
using Heyer.BuildingBlocks.Infrastructure.Integration.Processing;
using MediatR;

namespace Heyer.Modules.Hiring.Infrastructure.Integration;

internal class CompanyHiringOutboxProcessingJob : GenericOutboxProcessingJob
{
    public CompanyHiringOutboxProcessingJob(IMediator mediator,
                                            IUserDataProvider userDataProvider,
                                            IOutboxStore outboxStore)
        : base(mediator, userDataProvider, outboxStore)
    {
    }
}