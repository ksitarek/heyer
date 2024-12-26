using System.Net;
using FluentAssertions;
using Heyer.API.Tests.Utils;
using Heyer.BuildingBlocks.Domain.Tests.TestDataBuilders;
using Heyer.BuildingBlocks.Tests;
using Heyer.Modules.Hiring.Application;
using Heyer.Modules.Hiring.Domain.JobOffers;
using Heyer.Modules.Hiring.Infrastructure.Persistence;
using Heyer.Modules.Hiring.PublishedLanguage.DTOs;
using RestEase;

namespace Heyer.API.Tests.IntegrationTests.Endpoints;

[Category("Integration")]
public class GetJobOfferByIdEndpointTests : IntegrationTestsBase
{
    private HiringDbContext _ctx;
    private JobOfferDetails _expectedDetails;
    private JobOffer _jobOffer;

    [Test]
    public async Task GetJobOfferByIdEndpoint_ForOtherTenant_WillReturn404()
    {
        // Arrange
        var client = _appFactory.CreateAuthorizedApiClient(
            ApplicationFactoryConfiguration.Tenant2Id,
            HiringPermissions.ListJobOffers);

        // Act
        var action = async () => await client.GetJobOfferById(_jobOffer.Id.Guid);

        // Assert
        (await action.Should().ThrowAsync<ApiException>()).And.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task GetJobOfferByIdEndpoint_WillReturn404()
    {
        // Arrange
        var client = _appFactory.CreateAuthorizedApiClient(
            ApplicationFactoryConfiguration.Tenant1Id,
            HiringPermissions.ListJobOffers);

        // Act
        var action = async () => await client.GetJobOfferById(Guid.NewGuid());

        // Assert
        (await action.Should().ThrowAsync<ApiException>()).And.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task GetJobOfferByIdEndpoint_WithoutAuthorization_WillReturn401()
    {
        // Arrange
        var client = _appFactory.CreateApiClient();
        var jobOfferId = Guid.NewGuid();

        // Act
        var action = async () => await client.GetJobOfferById(jobOfferId);

        // Assert
        (await action.Should().ThrowAsync<ApiException>()).And.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task GetJobOfferByIdEndpoint_WithoutPermission_WillReturn403()
    {
        // Arrange
        var client = _appFactory.CreateAuthorizedApiClient(ApplicationFactoryConfiguration.Tenant1Id);
        var jobOfferId = Guid.NewGuid();

        // Act
        var action = async () => await client.GetJobOfferById(jobOfferId);

        // Assert
        (await action.Should().ThrowAsync<ApiException>()).And.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Test]
    public async Task GetJobOfferByIdEndpoint_WithPermission_WillReturn200()
    {
        // Arrange
        var client = _appFactory.CreateAuthorizedApiClient(
            ApplicationFactoryConfiguration.Tenant1Id,
            HiringPermissions.ListJobOffers);

        // Act
        var jobOffer = await client.GetJobOfferById(_jobOffer.Id.Guid);

        // Assert
        jobOffer.Should().NotBeNull().And.BeEquivalentTo(_expectedDetails);
    }

    [SetUp]
    public async Task SetUp()
    {
        _ctx = HiringDbContextProvider.Get(ApplicationFactoryConfiguration.Tenant1Id);

        _jobOffer = TestJobOfferBuilder.Create()
            .WithRandomContractDetails()
            .WithRandomOfficeLocation()
            .WithRandomRequirements()
            .Build();

        _expectedDetails = new JobOfferDetails(
            _jobOffer.Id.Guid,
            _jobOffer.OfferSummary,
            _jobOffer.JobDescription,
            _jobOffer.PublishedAt,
            _jobOffer.PublishedUntil,
            _jobOffer.Location!,
            _jobOffer.RemoteWork,
            _jobOffer.Requirements!,
            _jobOffer.ContractsDetails!.ToList());

        await _ctx.JobOffers.AddAsync(_jobOffer);

        await _ctx.SaveChangesAsync();
    }

    [TearDown]
    public async Task TearDown() => await _ctx.DisposeAsync();
}