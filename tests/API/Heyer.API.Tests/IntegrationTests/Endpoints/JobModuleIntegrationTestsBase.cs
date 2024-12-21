using Bogus;
using Heyer.API.Client;
using Heyer.BuildingBlocks.Tests;

namespace Heyer.API.Tests.IntegrationTests.Endpoints;

public abstract class JobModuleIntegrationTestsBase
{
    internal IApplicationFactory<IApiClient> AppFactory;
    internal Faker Faker = new();

    [SetUp]
    public virtual Task SetUpIntegrationTestsBase()
    {
        AppFactory = ApplicationFactory.Create();
        return Task.CompletedTask;
    }

    [TearDown]
    public async Task TearDownIntegrationTestsBase() => await AppFactory.DisposeAsync();
}