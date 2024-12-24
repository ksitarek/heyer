using System.Net;
using FluentAssertions;
using Heyer.API.Client;
using Heyer.Modules.Hiring.PublishedLanguage;
using Heyer.Storage.API.Client;

namespace Heyer.E2E.Tests;

[Category("E2E")]
public class HealthCheckTests
{
    private IApiClient _apiClient;
    private IStorageApiClient _storageApiClient;

    [Test]
    public async Task HealthCheck_ForAPI_ShouldBeOk()
    {
        // Arrange

        // Act
        var healthCheck = await _apiClient.HealthCheck();

        // Assert
        healthCheck.ResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);

        var report = healthCheck.GetContent();

        report.Status.Should().Be(HealthCheckStatus.Healthy);
        report.Results.Should().NotBeEmpty().And.HaveCount(1);
        report.Results.First().Key.Should().Be("JobBoardDatabase");
        report.Results.First().Value.Status.Should().Be(HealthCheckStatus.Healthy);
    }

    [Test]
    public async Task HealthCheck_ForStorageAPI_ShouldBeOk()
    {
        // Arrange

        // Act
        var healthCheck = await _storageApiClient.HealthCheck();

        // Assert
        healthCheck.ResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [SetUp]
    public void SetUp()
    {
        _apiClient = ApiClientFactory.Create(RuntimeSettings.ApiAddress, TimeSpan.FromSeconds(1));
        _storageApiClient =
            StorageApiClientFactory.Create(RuntimeSettings.StorageApiAddress, TimeSpan.FromSeconds(1));
    }
}