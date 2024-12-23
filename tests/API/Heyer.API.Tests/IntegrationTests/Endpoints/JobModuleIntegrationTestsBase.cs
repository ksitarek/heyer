using Bogus;
using Heyer.API.Client;
using Heyer.BuildingBlocks.Tests;
using Heyer.Modules.Hiring.Infrastructure;
using Heyer.Modules.JobBoard.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Heyer.API.Tests.IntegrationTests.Endpoints;

public abstract class JobModuleIntegrationTestsBase
{
    internal IApplicationFactory<IApiClient> AppFactory;

    internal Faker Faker = new();

    protected IServiceScope _hiringModuleCompositionRootScope;

    protected IServiceScope _jobBoardModuleCompositionRootScope;

    [SetUp]
    public virtual Task SetUpIntegrationTestsBase()
    {
        AppFactory = ApplicationFactory.Create();

        // hack making app factory to start immediately
        AppFactory.GetRequiredService<IConfiguration>();

        _hiringModuleCompositionRootScope = HiringModuleCompositionRoot.CreateScope();
        _jobBoardModuleCompositionRootScope = JobBoardModuleCompositionRoot.CreateScope();

        return Task.CompletedTask;
    }

    [TearDown]
    public async Task TearDownIntegrationTestsBase()
    {
        _hiringModuleCompositionRootScope.Dispose();
        _jobBoardModuleCompositionRootScope.Dispose();

        await AppFactory.DisposeAsync();
    }
}