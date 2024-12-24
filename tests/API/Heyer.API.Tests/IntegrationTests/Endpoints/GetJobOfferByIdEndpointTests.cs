using System.Net;
using FluentAssertions;
using Heyer.BuildingBlocks.Tests;
using Heyer.Modules.Hiring.Application;
using Heyer.Modules.Hiring.Domain.JobOffers;
using Heyer.Modules.Hiring.Infrastructure.Persistence;
using Heyer.Modules.Hiring.PublishedLanguage;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
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
        var client = AppFactory.CreateAuthorizedApiClient(
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
        var client = AppFactory.CreateAuthorizedApiClient(
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
        var client = AppFactory.CreateApiClient();
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
        var client = AppFactory.CreateAuthorizedApiClient(ApplicationFactoryConfiguration.Tenant1Id);
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
        var client = AppFactory.CreateAuthorizedApiClient(
            ApplicationFactoryConfiguration.Tenant1Id,
            HiringPermissions.ListJobOffers);

        // Act
        var jobOffer = await client.GetJobOfferById(_jobOffer.Id.Guid);

        // Assert
        jobOffer.Should().NotBeNull().And.BeEquivalentTo(_expectedDetails);
    }

    // todo test other tenant

    [SetUp]
    public async Task SetUp()
    {
        _ctx = GetContext(ApplicationFactoryConfiguration.Tenant1Id);

        _jobOffer = JobOffer.CreateNew(
            new CompanyDetails(Guid.NewGuid(), "ACME"),
            Faker.Random.String2(10, 100),
            Faker.Random.String2(100, 500),
            Faker.Random.Enum(RemoteWork.Unknown));

        _jobOffer.SetRequirements(ExperienceLevel.Junior,
                                  new Dictionary<string, SkillLevel>
                                  {
                                      ["A"] = SkillLevel.Mid, ["B"] = SkillLevel.Senior
                                  });

        _jobOffer.SetOfficeLocation(new OfficeLocation("Warsaw", "Poland"));

        _jobOffer.AddContractDetails(new ContractDetails(EmploymentType.B2B,
                                                         new SalaryRange(false, 10000, 20000),
                                                         8,
                                                         8));

        _expectedDetails = new JobOfferDetails(
            _jobOffer.Id.Guid,
            new CompanyDetails(_jobOffer.CompanyDetails.CompanyId,
                               _jobOffer.CompanyDetails.Name),
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


    private HiringDbContext GetContext(Guid companyId)
    {
        var connectionString =
            ApplicationFactoryConfiguration.InMemoryConfiguration[$"Companies:{companyId}:MongoDb:ConnectionString"];
        var databaseName =
            ApplicationFactoryConfiguration.InMemoryConfiguration[$"Companies:{companyId}:MongoDb:DatabaseName"];

        var client = new MongoClient(connectionString);
        var db = client.GetDatabase(databaseName);

        var options = new DbContextOptionsBuilder<HiringDbContext>()
            .UseMongoDB(db.Client, db.DatabaseNamespace.DatabaseName)
            .EnableServiceProviderCaching(false)
            .Options;

        return new HiringDbContext(options);
    }
}