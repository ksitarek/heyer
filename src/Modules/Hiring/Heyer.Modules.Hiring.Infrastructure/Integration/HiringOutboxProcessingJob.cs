using Hangfire;
using Heyer.BuildingBlocks.Application.Authorization;
using Heyer.BuildingBlocks.Infrastructure.Integration.Persistence;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Heyer.Modules.Hiring.Infrastructure.Integration;

internal class HiringOutboxProcessingJob
{
    private readonly IConfiguration _configuration;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public HiringOutboxProcessingJob(IConfiguration configuration) => _configuration = configuration;

    [DisableConcurrentExecution(100)]
    public async Task Handle()
    {
        await _semaphore.WaitAsync();
        var companiesSection = _configuration.GetSection("Companies");

        var companies = companiesSection.GetChildren();

        foreach (var configurationSection in companies)
        {
            var companyId = Guid.Parse(configurationSection.Key);

            using (var scope = HiringModuleCompositionRoot.CreateScope())
            {
                var userDataProvider =
                    scope.ServiceProvider.GetRequiredService<IUserDataProvider>() as ValueUserDataProvider;

                userDataProvider!.SetExecutionContext(Guid.Empty, companyId, string.Empty);

                var companyJob = new CompanyHiringOutboxProcessingJob(
                    scope.ServiceProvider.GetRequiredService<IMediator>(),
                    userDataProvider,
                    scope.ServiceProvider.GetRequiredService<IOutboxStore>());

                await companyJob.Handle();
            }
        }

        _semaphore.Release();
    }
}