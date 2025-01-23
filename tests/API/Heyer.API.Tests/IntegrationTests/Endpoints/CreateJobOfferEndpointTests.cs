using System.Net;
using Bogus;
using Heyer.API.Tests.Utils;
using Heyer.BuildingBlocks.Json;
using Heyer.BuildingBlocks.Tests;
using Heyer.Modules.Hiring.Application;
using Heyer.Modules.Hiring.PublishedLanguage.DTOs;
using Microsoft.AspNetCore.Mvc;
using RestEase;
using Shouldly;
using RemoteWork = Heyer.Modules.Hiring.PublishedLanguage.DTOs.RemoteWork;

namespace Heyer.API.Tests.IntegrationTests.Endpoints;

[Category("Integration")]
public class CreateJobOfferEndpointTests : IntegrationTestsBase
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
        var client = _appFactory.CreateAuthorizedApiClient(
            ApplicationFactoryConfiguration.Client1Id,
            HiringPermissions.UpdateJobOffer);
        var request = new CreateJobOfferRequest(offerSummary, jobDescription, remoteWork);

        // Act
        var action = async () => await client.CreateJobOffer(request);

        // Assert
        var exception = await action.ShouldThrowAsync<ApiException>();
        exception.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var validationDetails = exception.Content!.Deserialize<ValidationProblemDetails>()!;

        validationDetails.ShouldNotBeNull();
        validationDetails.Errors.Count.ShouldBe(1);
        validationDetails.Errors.Keys.ShouldContain(erroredField);
        validationDetails.Errors[erroredField].Length.ShouldBe(validationErrors.Length);
        foreach (var error in validationErrors)
        {
            validationDetails.Errors[erroredField].ShouldContain(error);
        }
    }

    [Test]
    public async Task CreateJobOfferEndpoint_WithoutAuthorization_WillReturn401()
    {
        // Arrange
        var client = _appFactory.CreateApiClient();
        var request = CreateJobOfferRequest();

        // Act
        var action = async () => await client.CreateJobOffer(request);

        // Assert
        var exception = await action.ShouldThrowAsync<ApiException>();
        exception.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task CreateJobOfferEndpoint_WithoutPermission_WillReturn403()
    {
        // Arrange
        var client = _appFactory.CreateAuthorizedApiClient(
            ApplicationFactoryConfiguration.Client1Id);
        var request = CreateJobOfferRequest();

        // Act
        var action = async () => await client.CreateJobOffer(request);

        // Assert
        var exception = await action.ShouldThrowAsync<ApiException>();
        exception.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }

    [Test]
    public async Task CreateJobOfferEndpoint_WithPermission_WillReturn200()
    {
        // Arrange
        var client = _appFactory.CreateAuthorizedApiClient(
            ApplicationFactoryConfiguration.Client1Id,
            HiringPermissions.UpdateJobOffer);
        var request = CreateJobOfferRequest();

        // Act
        var jobOfferId = await client.CreateJobOffer(request);

        // Assert
        await new JobOfferValidator(ApplicationFactoryConfiguration.Client1Id)
            .ValidateJobOfferIsSavedAsync(jobOfferId);
    }

    private CreateJobOfferRequest CreateJobOfferRequest()
    {
        var request = new CreateJobOfferRequest(
            _faker.Random.String(10, 100),
            _faker.Random.String(100, 500),
            _faker.Random.Enum(RemoteWork.Unknown));

        return request;
    }
}