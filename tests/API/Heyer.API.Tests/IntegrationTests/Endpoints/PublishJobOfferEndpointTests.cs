using System.Net;
using FluentAssertions;
using Heyer.API.Tests.Utils;
using Heyer.BuildingBlocks.Domain.Tests.TestDataBuilders;
using Heyer.BuildingBlocks.Tests;
using Heyer.Modules.Hiring.Application;
using Heyer.Modules.Hiring.Infrastructure.Persistence;
using RestEase;

namespace Heyer.API.Tests.IntegrationTests.Endpoints;

[Category("Integration")]
public class PublishJobOfferEndpointTests : IntegrationTestsBase
{
    private HiringDbContext _ctx;

    [Test]
    public async Task PublishJobOffer_WillReturn200()
    {
        // Arrange
        var client = _appFactory.CreateAuthorizedApiClient(
            ApplicationFactoryConfiguration.Tenant1Id,
            HiringPermissions.PublishJobOffer);

        var jobOffer = TestJobOfferBuilder.Create()
            .WithRandomContractDetails()
            .WithRandomOfficeLocation()
            .WithRandomRequirements()
            .Build();

        await _ctx.JobOffers.AddAsync(jobOffer);

        await _ctx.SaveChangesAsync();

        // Act
        await client.PublishJobOffer(jobOffer.Id.Guid);
        await AsyncHelper.AssertAllMessagesProcessed();

        // Assert
        var publishedOffer = await client.GetPublishedJobOfferById(jobOffer.Id.Guid);
        publishedOffer.Should().NotBeNull();
        publishedOffer.Id.Should().Be(jobOffer.Id.Guid);
    }

    [Test]
    public async Task PublishJobOffer_WillReturn401()
    {
        // Arrange
        var client = _appFactory.CreateApiClient();

        // Act
        var action = async () => await client.PublishJobOffer(Guid.NewGuid());

        // Assert
        (await action.Should().ThrowAsync<ApiException>()).And.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task PublishJobOffer_WillReturn403()
    {
        // Arrange
        var client = _appFactory.CreateAuthorizedApiClient(ApplicationFactoryConfiguration.Tenant1Id);

        // Act
        var action = async () => await client.PublishJobOffer(Guid.NewGuid());

        // Assert
        (await action.Should().ThrowAsync<ApiException>()).And.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Test]
    public async Task PublishJobOffer_WillReturn404()
    {
        // Arrange
        var client = _appFactory.CreateAuthorizedApiClient(
            ApplicationFactoryConfiguration.Tenant1Id,
            HiringPermissions.PublishJobOffer);

        // Act
        var action = async () => await client.PublishJobOffer(Guid.NewGuid());

        // Assert
        (await action.Should().ThrowAsync<ApiException>()).And.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [SetUp]
    public void SetUp() =>
        _ctx = HiringDbContextProvider.Get(ApplicationFactoryConfiguration.Tenant1Id);

    [TearDown]
    public async Task TearDown() => await _ctx.DisposeAsync();
}