using System.Net;
using FluentAssertions;
using Heyer.API.Client.PublishedLanguage;
using Heyer.Modules.JobBoard.Application;
using RestEase;

namespace Heyer.API.Tests.IntegrationTests.Endpoints;

[Category("Integration")]
public class CreateJobOfferEndpointTests : JobModuleIntegrationTestsBase
{
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
        var action = async () => await client.CreateJobOffer(request);

        // Assert
        await action.Should().NotThrowAsync<ApiException>();
    }

    private static CreateJobOfferRequest CreateJobOfferRequest()
    {
        var request = new CreateJobOfferRequest(
            "offer-summary",
            "job-description",
            RemoteWork.No);
        return request;
    }
}