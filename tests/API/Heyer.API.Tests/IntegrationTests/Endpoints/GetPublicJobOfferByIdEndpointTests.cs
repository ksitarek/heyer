using System.Net;
using FluentAssertions;
using Heyer.Modules.Hiring.PublishedLanguage;
using Heyer.Modules.JobBoard.Domain.JobOffers;
using Heyer.Modules.JobBoard.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
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
        var client = AppFactory.CreateApiClient();

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
        var client = AppFactory.CreateApiClient();

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

        var client = AppFactory.CreateApiClient();

        // Act
        var action = async () => await client.GetPublishedJobOfferById(_publishedJobOffer.Id.Guid);

        // Assert
        (await action.Should().ThrowAsync<ApiException>())
            .And.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [SetUp]
    public async Task SetUp()
    {
        _ctx = _jobBoardModuleCompositionRootScope.ServiceProvider.GetRequiredService<JobBoardContext>();

        _publishedJobOffer = PublishedJobOffer.CreateNew(
            new CompanyDetails(Guid.NewGuid(), "ACME"),
            Faker.Random.String2(10, 100),
            Faker.Random.String2(100, 500),
            Faker.Random.Enum(RemoteWork.Unknown));

        _publishedJobOffer.SetRequirements(ExperienceLevel.Junior,
                                           new Dictionary<string, SkillLevel>
                                           {
                                               ["A"] = SkillLevel.Mid, ["B"] = SkillLevel.Senior
                                           });

        _publishedJobOffer.SetOfficeLocation(new OfficeLocation("Warsaw", "Poland"));

        _publishedJobOffer.AddContractDetails(new ContractDetails(EmploymentType.B2B,
                                                                  new SalaryRange(false, 10000, 20000),
                                                                  8,
                                                                  8));

        _expectedDetails = new PublishedJobOfferDetails(
            _publishedJobOffer.Id.Guid,
            new CompanyDetails(_publishedJobOffer.CompanyDetails.CompanyId,
                               _publishedJobOffer.CompanyDetails.Name),
            _publishedJobOffer.OfferSummary,
            _publishedJobOffer.JobDescription,
            _publishedJobOffer.Location!,
            _publishedJobOffer.RemoteWork,
            _publishedJobOffer.Requirements!,
            new List<ContractDetails>
            {
                new(
                    _publishedJobOffer.ContractsDetails!.First().EmploymentType,
                    new SalaryRange(false, 0, 0),
                    _publishedJobOffer.ContractsDetails!.First().TimeNumerator,
                    _publishedJobOffer.ContractsDetails!.First().TimeDenominator)
            });

        await _ctx.PublishedJobOffers.AddAsync(_publishedJobOffer);

        await _ctx.SaveChangesAsync();
    }

    [TearDown]
    public async Task TearDown() => await _ctx.DisposeAsync();
}