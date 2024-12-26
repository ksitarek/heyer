using Heyer.BuildingBlocks.Application.Authorization;
using Heyer.BuildingBlocks.Infrastructure.Integration.Persistence;
using Heyer.BuildingBlocks.Infrastructure.Integration.Processing;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Heyer.Modules.JobBoard.Infrastructure.Integration;

internal static class JobBoardOutboxProcessingJob
{
    public static async Task RunAsync()
    {
        using var scope = JobBoardModuleCompositionRoot.CreateScope();
        var jobRun = scope.ServiceProvider.GetService<JobBoardOutboxProcessingJobRun>();
        await jobRun!.Handle();
    }
}

internal class JobBoardOutboxProcessingJobRun : GenericOutboxProcessingJob
{
    public JobBoardOutboxProcessingJobRun(IMediator mediator,
                                          IUserDataProvider userDataProvider,
                                          IOutboxStore outboxStore)
        : base(mediator, userDataProvider, outboxStore)
    {
    }
}