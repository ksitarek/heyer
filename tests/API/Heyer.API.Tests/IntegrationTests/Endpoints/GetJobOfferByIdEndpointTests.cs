using System.Net;
using Heyer.API.Tests.Utils;
using Heyer.BuildingBlocks.Domain.Tests.TestDataBuilders;
using Heyer.BuildingBlocks.Tests;
using Heyer.Modules.Hiring.Application;
using Heyer.Modules.Hiring.Domain.JobOffers;
using Heyer.Modules.Hiring.Infrastructure.Persistence;
using Heyer.Modules.Hiring.PublishedLanguage.DTOs;
using RestEase;
using Shouldly;

namespace Heyer.API.Tests.IntegrationTests.Endpoints;

[Category("Integration")]
public class GetJobOfferByIdEndpointTests : IntegrationTestsBase
{
    private HiringDbContext _ctx = null!;
    private JobOfferDetails _expectedDetails = null!;
    private JobOffer _jobOffer = null!;

    [Test]
    public async Task GetJobOfferByIdEndpoint_ForOtherClient_WillReturn404()
    {
        // Arrange
        var client = _appFactory.CreateAuthorizedApiClient(
            ApplicationFactoryConfiguration.Client2Id,
            HiringPermissions.ListJobOffers);

        // Act
        var action = async () => await client.GetJobOfferById(_jobOffer.Id.Guid);

        // Assert
        var exception = await action.ShouldThrowAsync<ApiException>();
        exception.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task GetJobOfferByIdEndpoint_WillReturn404()
    {
        // Arrange
        var client = _appFactory.CreateAuthorizedApiClient(
            ApplicationFactoryConfiguration.Client1Id,
            HiringPermissions.ListJobOffers);

        // Act
        var action = async () => await client.GetJobOfferById(Guid.NewGuid());

        // Assert
        var exception = await action.ShouldThrowAsync<ApiException>();
        exception.StatusCode.ShouldBe(HttpStatusCode.NotFound);
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
        var exception = await action.ShouldThrowAsync<ApiException>();
        exception.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task GetJobOfferByIdEndpoint_WithoutPermission_WillReturn403()
    {
        // Arrange
        var client = _appFactory.CreateAuthorizedApiClient(ApplicationFactoryConfiguration.Client1Id);
        var jobOfferId = Guid.NewGuid();

        // Act
        var action = async () => await client.GetJobOfferById(jobOfferId);

        // Assert
        var exception = await action.ShouldThrowAsync<ApiException>();
        exception.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }

    [Test]
    public async Task GetJobOfferByIdEndpoint_WithPermission_WillReturn200()
    {
        // Arrange
        var client = _appFactory.CreateAuthorizedApiClient(
            ApplicationFactoryConfiguration.Client1Id,
            HiringPermissions.ListJobOffers);

        // Act
        var jobOffer = await client.GetJobOfferById(_jobOffer.Id.Guid);

        // Assert
        jobOffer.ShouldNotBeNull();
        jobOffer.ShouldBeEquivalentTo(_expectedDetails);
    }

    [SetUp]
    public async Task SetUp()
    {
        _ctx = HiringDbContextProvider.Get(ApplicationFactoryConfiguration.Client1Id);

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

        await _ctx.Set<JobOffer>().AddAsync(_jobOffer);

        await _ctx.SaveChangesAsync();
    }

    [TearDown]
    public async Task TearDown() => await _ctx.DisposeAsync();
}