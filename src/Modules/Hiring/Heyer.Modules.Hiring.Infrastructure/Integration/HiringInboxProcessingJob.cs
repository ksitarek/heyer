using Hangfire;
using Heyer.BuildingBlocks.Application.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Heyer.Modules.Hiring.Infrastructure.Integration;

internal class HiringInboxProcessingJob
{
    private readonly IConfiguration _configuration;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public HiringInboxProcessingJob(IConfiguration configuration) =>
        _configuration = configuration;

    [DisableConcurrentExecution(100)]
    public async Task Handle()
    {
        var companiesSection = _configuration.GetSection("Companies");

        var companies = companiesSection.GetChildren();

        foreach (var configurationSection in companies)
        {
            var companyId = Guid.Parse(configurationSection.Key);

            using (var scope = HiringModuleCompositionRoot.CreateScope())
            {
                await _semaphore.WaitAsync();
                var userDataProvider =
                    scope.ServiceProvider.GetRequiredService<IUserDataProvider>() as ValueUserDataProvider;

                userDataProvider!.SetExecutionContext(Guid.Empty, companyId, string.Empty);

                var companyJob = new CompanyHiringInboxProcessingJob(scope);

                await companyJob.Handle();
                _semaphore.Release();
            }
        }
    }
}