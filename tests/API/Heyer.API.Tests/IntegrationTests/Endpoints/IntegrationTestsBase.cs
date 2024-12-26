using Bogus;
using Heyer.API.Client;
using Heyer.BuildingBlocks.Tests;
using Heyer.Modules.Hiring.Infrastructure;
using Heyer.Modules.JobBoard.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Heyer.API.Tests.IntegrationTests.Endpoints;

public abstract class IntegrationTestsBase
{
    internal IApplicationFactory<IApiClient> _appFactory;

    internal readonly Faker _faker = new();

    protected IServiceScope HiringModuleCompositionRootScope;

    protected IServiceScope JobBoardModuleCompositionRootScope;

    [OneTimeSetUp]
    public virtual Task SetUpIntegrationTestsBase()
    {
        _appFactory = ApplicationFactory.Create();

        // hack making app factory to start immediately
        _appFactory.GetRequiredService<IConfiguration>();

        HiringModuleCompositionRootScope = HiringModuleCompositionRoot.CreateScope();
        JobBoardModuleCompositionRootScope = JobBoardModuleCompositionRoot.CreateScope();

        return Task.CompletedTask;
    }

    [OneTimeTearDown]
    public async Task TearDownIntegrationTestsBase()
    {
        HiringModuleCompositionRootScope.Dispose();
        JobBoardModuleCompositionRootScope.Dispose();

        await _appFactory.DisposeAsync();
    }
}