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