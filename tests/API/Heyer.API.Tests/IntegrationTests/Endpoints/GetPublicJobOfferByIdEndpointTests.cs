using System.Net;
using FluentAssertions;
using Heyer.BuildingBlocks.Domain.Tests.TestDataBuilders;
using Heyer.BuildingBlocks.Tests;
using Heyer.Modules.Hiring.PublishedLanguage.DTOs;
using Heyer.Modules.JobBoard.Domain.JobOffers;
using Heyer.Modules.JobBoard.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using RestEase;

namespace Heyer.API.Tests.IntegrationTests.Endpoints;

[Category("Integration")]
public class GetPublicJobOfferByIdEndpointTests : IntegrationTestsBase
{
    private JobBoardContext _ctx;
    private PublishedJobOfferDetails _expectedDetails;
    private PublishedJobOffer _publishedJobOffer = null!;

    [Test]
    public async Task GetJobOfferByIdEndpoint_WillReturn200Ok_WhenOfferFound()
    {
        // Arrange
        var client = _appFactory.CreateApiClient();

        // Act
        var jobOffer = await client.GetPublishedJobOfferById(_publishedJobOffer.Id.Guid);

        // Assert
        jobOffer.Should().NotBeNull();
        jobOffer.Should()
            .BeEquivalentTo(
                _expectedDetails);
    }

    [Test]
    public async Task GetJobOfferByIdEndpoint_WillReturn404NotFound_WhenOfferNotFound()
    {
        // Arrange
        var client = _appFactory.CreateApiClient();

        // Act
        var action = async () => await client.GetPublishedJobOfferById(Guid.NewGuid());

        // Assert
        (await action.Should().ThrowAsync<ApiException>())
            .And.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task GetJobOfferByIdEndpoint_WillReturn404NotFound_WhenOfferWasTakenDown()
    {
        // Arrange
        _publishedJobOffer.TakeDown();
        await _ctx.SaveChangesAsync();

        var client = _appFactory.CreateApiClient();

        // Act
        var action = async () => await client.GetPublishedJobOfferById(_publishedJobOffer.Id.Guid);

        // Assert
        (await action.Should().ThrowAsync<ApiException>())
            .And.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [SetUp]
    public async Task SetUp()
    {
        _ctx = GetContext();

        _publishedJobOffer = TestPublishedJobOfferBuilder.Create(ApplicationFactoryConfiguration.Tenant1Id)
            .BuildTestData();

        _expectedDetails = new PublishedJobOfferDetails(
            _publishedJobOffer.Id.Guid,
            new CompanyDetails(_publishedJobOffer.CompanyDetails.CompanyId,
                               _publishedJobOffer.CompanyDetails.Name),
            _publishedJobOffer.OfferSummary,
            _publishedJobOffer.JobDescription,
            _publishedJobOffer.Location,
            _publishedJobOffer.RemoteWork,
            _publishedJobOffer.Requirements,
            new List<ContractDetails>
            {
                new(
                    _publishedJobOffer.ContractsDetails.First().EmploymentType,
                    _publishedJobOffer.ContractsDetails.First().SalaryRange.IsPublished
                        ? _publishedJobOffer.ContractsDetails.First().SalaryRange
                        : new SalaryRange(false, 0, 0),
                    _publishedJobOffer.ContractsDetails.First().TimeNumerator,
                    _publishedJobOffer.ContractsDetails.First().TimeDenominator)
            });

        await _ctx.PublishedJobOffers.AddAsync(_publishedJobOffer);

        await _ctx.SaveChangesAsync();
    }

    [TearDown]
    public async Task TearDown() => await _ctx.DisposeAsync();

    private JobBoardContext GetContext()
    {
        var db = JobBoardModuleCompositionRootScope.ServiceProvider.GetRequiredService<IMongoDatabase>();

        var options = new DbContextOptionsBuilder<JobBoardContext>()
            .UseMongoDB(db.Client, db.DatabaseNamespace.DatabaseName)
            .ConfigureWarnings(x => x.Ignore(CoreEventId.ManyServiceProvidersCreatedWarning))
            .EnableServiceProviderCaching(false)
            .Options;

        return new JobBoardContext(options);
    }
}