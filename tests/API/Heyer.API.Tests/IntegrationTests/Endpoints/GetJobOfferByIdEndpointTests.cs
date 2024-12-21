using System.Net;
using FluentAssertions;
using Heyer.API.Client.PublishedLanguage;
using Heyer.Modules.JobBoard.Domain.Companies;
using Heyer.Modules.JobBoard.Domain.JobOffers;
using Heyer.Modules.JobBoard.Infrastructure.Persistence;
using RestEase;
using RemoteWork = Heyer.Modules.JobBoard.Domain.JobOffers.RemoteWork;

namespace Heyer.API.Tests.IntegrationTests.Endpoints;

[Category("Integration")]
public class GetJobOfferByIdEndpointTests : JobModuleIntegrationTestsBase
{
    private JobBoardContext _ctx;
    private JobOfferDetails _expectedDetails;
    private JobOffer _jobOffer = null!;

    [Test]
    public async Task GetJobOfferByIdEndpoint_WillReturn200Ok_WhenOfferFound()
    {
        // Arrange
        var client = AppFactory.CreateApiClient();

        // Act
        var jobOffer = await client.GetJobOfferById(_jobOffer.Id.Guid);

        // Assert
        jobOffer.Should().NotBeNull();
        jobOffer.Should()
            .BeEquivalentTo(
                _expectedDetails);
    }

    [Test]
    public async Task GetJobOfferByIdEndpoint_WillReturn404NotFound_WhenOfferIsExpired()
    {
        // Arrange
        _jobOffer.TakeDown();
        _jobOffer.Publish(DateTimeOffset.Now.AddDays(-1));
        await _ctx.SaveChangesAsync();

        var client = AppFactory.CreateApiClient();

        // Act
        var action = async () => await client.GetJobOfferById(_jobOffer.Id.Guid);

        // Assert
        (await action.Should().ThrowAsync<ApiException>())
            .And.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task GetJobOfferByIdEndpoint_WillReturn404NotFound_WhenOfferIsNotPublished()
    {
        // Arrange
        _jobOffer.TakeDown();
        await _ctx.SaveChangesAsync();

        var client = AppFactory.CreateApiClient();

        // Act
        var action = async () => await client.GetJobOfferById(_jobOffer.Id.Guid);

        // Assert
        (await action.Should().ThrowAsync<ApiException>())
            .And.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task GetJobOfferByIdEndpoint_WillReturn404NotFound_WhenOfferNotFound()
    {
        // Arrange
        var client = AppFactory.CreateApiClient();

        // Act
        var action = async () => await client.GetJobOfferById(Guid.NewGuid());

        // Assert
        (await action.Should().ThrowAsync<ApiException>())
            .And.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [SetUp]
    public async Task SetUp()
    {
        _ctx = AppFactory.GetRequiredService<JobBoardContext>();

        _jobOffer = JobOffer.CreateNew(
            new CompanyDetails(CompanyId.CreateNew(), "ACME"),
            Faker.Random.String2(10, 100),
            Faker.Random.String2(100, 500),
            Faker.Random.Enum(RemoteWork.Unknown));

        _jobOffer.SetRequirements(ExperienceLevel.Junior,
                                  new Dictionary<string, SkillLevel>
                                  {
                                      ["A"] = SkillLevel.Mid, ["B"] = SkillLevel.Senior
                                  });

        _jobOffer.SetOfficeLocation(new OfficeLocation("Warsaw", "Poland"));

        _jobOffer.AddContractDetails(new ContractDetails(EmploymentType.ContractOfEmployment,
                                                         new SalaryRange(false, 10000, 20000),
                                                         8,
                                                         8));

        _jobOffer.Publish();

        _expectedDetails = new JobOfferDetails(
            _jobOffer.Id.Guid,
            new JobOfferDetails.CompanyDetailsDto(_jobOffer.CompanyDetails.CompanyId.Id, _jobOffer.CompanyDetails.Name),
            _jobOffer.OfferSummary,
            _jobOffer.JobDescription,
            new JobOfferDetails.LocationDto(_jobOffer.Location!.City, _jobOffer.Location!.Country),
            Map(_jobOffer.RemoteWork),
            new JobOfferDetails.RequirementsDto(
                Map(_jobOffer.Requirements!.ExperienceLevel),
                _jobOffer.Requirements.Skills!.Select(x => new JobOfferDetails.SkillDto(x.Label, Map(x.Level)))
                    .ToList()),
            _jobOffer.ContractsDetails!.Select(x => Map(x)).ToList()
        );

        await _ctx.JobOffers.AddAsync(_jobOffer);

        await _ctx.SaveChangesAsync();
    }

    [TearDown]
    public async Task TearDown() => await _ctx.DisposeAsync();

    private JobOfferDetails.ContractDetailsDto Map(ContractDetails contractDetails)
        => new(
            Map(contractDetails.EmploymentType),
            Map(contractDetails.SalaryRange));

    private JobOfferDetails.SalaryRangeDto Map(SalaryRange salaryRange) => new(salaryRange.From, salaryRange.To);

    private JobOfferDetails.EmploymentType Map(EmploymentType employmentType)
        => employmentType switch
        {
            EmploymentType.ContractOfEmployment => JobOfferDetails.EmploymentType.ContractOfEmployment,
            EmploymentType.B2B => JobOfferDetails.EmploymentType.B2B,
            _ => throw new ArgumentOutOfRangeException(nameof(employmentType), employmentType, null)
        };

    private JobOfferDetails.SkillLevel Map(SkillLevel skillLevel)
        => skillLevel switch
        {
            SkillLevel.Junior => JobOfferDetails.SkillLevel.Junior,
            SkillLevel.Mid => JobOfferDetails.SkillLevel.Mid,
            SkillLevel.Senior => JobOfferDetails.SkillLevel.Senior,
            _ => throw new ArgumentOutOfRangeException(nameof(skillLevel), skillLevel, null)
        };

    private JobOfferDetails.ExperienceLevel Map(ExperienceLevel experienceLevel)
        => experienceLevel switch
        {
            ExperienceLevel.Junior => JobOfferDetails.ExperienceLevel.Junior,
            ExperienceLevel.Mid => JobOfferDetails.ExperienceLevel.Mid,
            ExperienceLevel.Senior => JobOfferDetails.ExperienceLevel.Senior,
            _ => throw new ArgumentOutOfRangeException(nameof(experienceLevel), experienceLevel, null)
        };

    private Client.PublishedLanguage.RemoteWork Map(RemoteWork remoteWork) =>
        remoteWork switch
        {
            RemoteWork.Unknown => Client.PublishedLanguage.RemoteWork.Unknown,
            RemoteWork.Yes => Client.PublishedLanguage.RemoteWork.Yes,
            RemoteWork.Hybrid => Client.PublishedLanguage.RemoteWork.Hybrid,
            RemoteWork.No => Client.PublishedLanguage.RemoteWork.No,
            _ => throw new ArgumentOutOfRangeException(nameof(remoteWork), remoteWork, null)
        };
}