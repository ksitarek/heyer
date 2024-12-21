using System.Net;
using System.Text.Json;
using Bogus;
using FluentAssertions;
using Heyer.API.Client.PublishedLanguage;
using Heyer.API.Tests.Utils;
using Heyer.Modules.JobBoard.Application;
using Heyer.Modules.JobBoard.Domain.Companies;
using Heyer.Modules.JobBoard.Domain.JobOffers;
using Heyer.Modules.JobBoard.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using RestEase;
using RemoteWork = Heyer.API.Client.PublishedLanguage.RemoteWork;

namespace Heyer.API.Tests.IntegrationTests.Endpoints;

[Category("Integration")]
public class CreateJobOfferEndpointTests : JobModuleIntegrationTestsBase
{
    public static IEnumerable<object[]> BadRequestTestCases()
    {
        var f = new Faker();

        var validOfferSummary = "umcxmhkhhdvqmmyklfjx";
        var tooShortOfferSummary = "rm";
        var tooLongOfferSummary =
            "leutzowmnireuzitefewwwmxjfbhoaokztnskokbyaqewzjjlobovdyeethytiiqhhjhaytckwzbtjleleattpnjbsllxnvmjqfjlsuuzlbrovwnazlsepdyobortwstvdvrtyurponpsxmgqmhppqdtnegjgtfkwgkrbqjzvrxcxvishbszuplmxjaatzcgfeqwtwskcatdgmmvsfpuchqzjdklxskgkowuasmjgmuyjhqguqmpgmivodioezxrsnrslwfmcwareichjryhiooxcbuqxsivszdcrolumtvefzassubzvrlclyhqftrogljvqkledzygnpripqdgqwkhpflyygpdnwhdktkjynkrfiffmpqdkozqbkahionjkybfpzqzadblcmvwpsjbwdviljwvveocmupncdvdeqhqdxugorzghuqyfanypnffwvxzurrcfeljqubqwhwetkvmwc";

        var validJobDescription =
            "saacpecnienlzckquxujarumjjothtjzewflnjfrjcsemhdqxjybttlurbgkongmaorbedvmoocpibvbwxfljntgcvplywqgipiryoxapjvphpwtrqmuqavehdkjvadf";
        var tooShortJobDescription = "wyqxu";

        var validRemoteWork = f.Random.Enum(RemoteWork.Unknown);

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        yield return
        [
            null, validJobDescription, validRemoteWork, "OfferSummary", new[] { "'Offer Summary' must not be empty." }
        ];
        yield return
        [
            string.Empty, validJobDescription, validRemoteWork, "OfferSummary",
            new[]
            {
                "'Offer Summary' must not be empty.",
                "The length of 'Offer Summary' must be at least 10 characters. You entered 0 characters."
            }
        ];
        yield return
        [
            tooShortOfferSummary, validJobDescription, validRemoteWork, "OfferSummary",
            new[]
            {
                $"The length of 'Offer Summary' must be at least 10 characters. You entered {tooShortOfferSummary.Length} characters."
            }
        ];
        yield return
        [
            tooLongOfferSummary, validJobDescription, validRemoteWork, "OfferSummary",
            new[]
            {
                $"The length of 'Offer Summary' must be 100 characters or fewer. You entered {tooLongOfferSummary.Length} characters."
            }
        ];

        yield return
        [
            validOfferSummary, null, validRemoteWork, "JobDescription", new[] { "'Job Description' must not be empty." }
        ];
        yield return
        [
            validOfferSummary, string.Empty, validRemoteWork, "JobDescription",
            new[]
            {
                "'Job Description' must not be empty.",
                "The length of 'Job Description' must be at least 100 characters. You entered 0 characters."
            }
        ];
        yield return
        [
            validOfferSummary, tooShortJobDescription, validRemoteWork, "JobDescription",
            new[]
            {
                $"The length of 'Job Description' must be at least 100 characters. You entered {tooShortJobDescription.Length} characters."
            }
        ];

        yield return
            [validOfferSummary, validJobDescription, null, "RemoteWork", new[] { "Remote work must be specified." }];
        yield return
        [
            validOfferSummary, validJobDescription, RemoteWork.Unknown, "RemoteWork",
            new[] { "Remote work must be specified." }
        ];
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
    }

    [Theory]
    [TestCaseSource(nameof(BadRequestTestCases))]
    public async Task CreateJobOfferEndpoint_WithInvalidData_WillReturn400(
        string offerSummary,
        string jobDescription,
        RemoteWork remoteWork,
        string erroredField,
        string[] validationErrors)
    {
        // Arrange
        var client = AppFactory.CreateAuthorizedApiClient(JobBoardPermissions.CreateJobOffer);
        var request = new CreateJobOfferRequest(offerSummary, jobDescription, remoteWork);

        // Act
        var action = async () => await client.CreateJobOffer(request);

        // Assert
        var exception = (await action.Should().ThrowAsync<ApiException>()).Subject.First();
        exception.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var validationDetails = JsonSerializer.Deserialize<ValidationProblemDetails>(exception.Content!)!;

        validationDetails.Should().NotBeNull();
        validationDetails.Errors.Should().HaveCount(1).And.ContainKeys(erroredField);
        validationDetails.Errors[erroredField].Should().HaveCount(validationErrors.Length);
        foreach (var error in validationErrors)
        {
            validationDetails.Errors[erroredField].Should().Contain(error);
        }
    }

    [Test]
    public async Task CreateJobOfferEndpoint_WithoutAuthorization_WillReturn401()
    {
        // Arrange
        var client = AppFactory.CreateApiClient();
        var request = CreateJobOfferRequest();

        // Act
        var action = async () => await client.CreateJobOffer(request);

        // Assert
        (await action.Should().ThrowAsync<ApiException>()).And.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task CreateJobOfferEndpoint_WithoutPermission_WillReturn403()
    {
        // Arrange
        var client = AppFactory.CreateAuthorizedApiClient();
        var request = CreateJobOfferRequest();

        // Act
        var action = async () => await client.CreateJobOffer(request);

        // Assert
        (await action.Should().ThrowAsync<ApiException>()).And.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Test]
    public async Task CreateJobOfferEndpoint_WithPermission_WillReturn200()
    {
        // Arrange
        var client = AppFactory.CreateAuthorizedApiClient(JobBoardPermissions.CreateJobOffer);
        var request = CreateJobOfferRequest();

        // Act
        var jobOfferId = await client.CreateJobOffer(request);

        // Assert
        await AppFactory.GetRequiredService<JobOfferValidator>()
            .ValidateJobOfferIsSavedAsync(jobOfferId);
    }

    private CreateJobOfferRequest CreateJobOfferRequest()
    {
        var request = new CreateJobOfferRequest(
            Faker.Random.String(10, 100),
            Faker.Random.String(100, 500),
            Faker.Random.Enum(RemoteWork.Unknown));

        return request;
    }
}

[Category("Integration")]
public class GetJobOfferByIdEndpointTests : JobModuleIntegrationTestsBase
{
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
                new JobOfferDetails(_jobOffer.Id.Guid, _jobOffer.GetOfferSummary(), _jobOffer.GetJobDescription()));
    }

    [SetUp]
    public async Task SetUp()
    {
        await using var ctx = AppFactory.GetRequiredService<JobBoardContext>();

        _jobOffer = JobOffer.CreateNew(
            new CompanyDetails(CompanyId.CreateNew(), "ACME"),
            Faker.Random.String2(10, 100),
            Faker.Random.String2(100, 500),
            Faker.Random.Enum(Modules.JobBoard.Domain.JobOffers.RemoteWork.Unknown));

        _jobOffer.SetRequirements(ExperienceLevel.Junior,
                                  new Dictionary<string, SkillLevel>
                                  {
                                      ["A"] = SkillLevel.Mid, ["B"] = SkillLevel.Senior
                                  });

        _jobOffer.SetOfficeLocation(new OfficeLocation("Warsaw", "Poland"));

        _jobOffer.Publish(DateTimeOffset.Now);

        await ctx.JobOffers.AddAsync(_jobOffer);

        await ctx.SaveChangesAsync();
    }
}