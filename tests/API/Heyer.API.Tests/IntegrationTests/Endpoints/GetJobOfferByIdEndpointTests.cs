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
    private PublishedJobOffer _publishedJobOffer = null!;

    [Test]
    public async Task GetJobOfferByIdEndpoint_WillReturn200Ok_WhenOfferFound()
    {
        // Arrange
        var client = AppFactory.CreateApiClient();

        // Act
        var jobOffer = await client.GetJobOfferById(_publishedJobOffer.Id.Guid);

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
        var action = async () => await client.GetJobOfferById(Guid.NewGuid());

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
        var action = async () => await client.GetJobOfferById(_publishedJobOffer.Id.Guid);

        // Assert
        (await action.Should().ThrowAsync<ApiException>())
            .And.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [SetUp]
    public async Task SetUp()
    {
        _ctx = AppFactory.GetRequiredService<JobBoardContext>();

        _publishedJobOffer = PublishedJobOffer.CreateNew(
            new CompanyDetails(CompanyId.CreateNew(), "ACME"),
            Faker.Random.String2(10, 100),
            Faker.Random.String2(100, 500),
            Faker.Random.Enum(RemoteWork.Unknown));

        _publishedJobOffer.SetRequirements(ExperienceLevel.Junior,
                                           new Dictionary<string, SkillLevel>
                                           {
                                               ["A"] = SkillLevel.Mid, ["B"] = SkillLevel.Senior
                                           });

        _publishedJobOffer.SetOfficeLocation(new OfficeLocation("Warsaw", "Poland"));

        _publishedJobOffer.AddContractDetails(new ContractDetails(EmploymentType.ContractOfEmployment,
                                                                  new SalaryRange(false, 10000, 20000),
                                                                  8,
                                                                  8));

        _expectedDetails = new JobOfferDetails(
            _publishedJobOffer.Id.Guid,
            new JobOfferDetails.CompanyDetailsDto(_publishedJobOffer.CompanyDetails.CompanyId.Id,
                                                  _publishedJobOffer.CompanyDetails.Name),
            _publishedJobOffer.OfferSummary,
            _publishedJobOffer.JobDescription,
            new JobOfferDetails.LocationDto(_publishedJobOffer.Location!.City, _publishedJobOffer.Location!.Country),
            Map(_publishedJobOffer.RemoteWork),
            new JobOfferDetails.RequirementsDto(
                Map(_publishedJobOffer.Requirements!.ExperienceLevel),
                _publishedJobOffer.Requirements.Skills!.Select(x => new JobOfferDetails.SkillDto(x.Label, Map(x.Level)))
                    .ToList()),
            _publishedJobOffer.ContractsDetails!.Select(x => Map(x)).ToList()
        );

        await _ctx.PublishedJobOffers.AddAsync(_publishedJobOffer);

        await _ctx.SaveChangesAsync();
    }

    [TearDown]
    public async Task TearDown() => await _ctx.DisposeAsync();

    private JobOfferDetails.ContractDetailsDto Map(ContractDetails contractDetails)
        => new(
            Map(contractDetails.EmploymentType),
            contractDetails.SalaryRange.IsPublished ? Map(contractDetails.SalaryRange) : null);

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