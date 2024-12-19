using System.Net;
using System.Text.Json;
using Bogus;
using FluentAssertions;
using Heyer.API.Client.PublishedLanguage;
using Heyer.API.Tests.Utils;
using Heyer.Modules.JobBoard.Application;
using Microsoft.AspNetCore.Mvc;
using RestEase;

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
        string offerSummary, string jobDescription, RemoteWork remoteWork, string erroredField,
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