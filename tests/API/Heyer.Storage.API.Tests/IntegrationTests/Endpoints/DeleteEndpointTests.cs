using System.Net;
using FluentAssertions;
using Heyer.Storage.API.Tests.Utils;
using Heyer.Storage.API.Tests.Utils.Validators;
using RestEase;

namespace Heyer.Storage.API.Tests.IntegrationTests.Endpoints;

[Category("Integration")]
public class DeleteEndpointTests : StorageApiIntegrationTestsBase
{
    [Test]
    public async Task DownloadEndpoint_WithoutAuthorization_WillReturn401()
    {
        // Arrange
        var client = AppFactory.CreateApiClient();

        // Act
        var action = async () => await client.Delete(Guid.NewGuid().ToString());

        // Assert
        (await action.Should().ThrowAsync<ApiException>()).And.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task DownloadEndpoint_WithInvalidKey_WillReturnOk()
    {
        // Arrange
        var client = AppFactory.CreateAuthorizedApiClient();

        // Act
        var action = async () => await client.Delete(Guid.NewGuid().ToString());

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Test]
    public async Task DownloadEndpoint_WithValidKey_WillReturnOk()
    {
        // Arrange
        var client = AppFactory.CreateAuthorizedApiClient();
        var storeResult = await client.Store("Utils/TestFiles/test-file.png");

        // Act
        await client.Delete(storeResult.FileHandle);

        // Assert
        await AppFactory.GetRequiredService<IStorageStrategyValidator>()
            .ValidateFileIsNotPresent(key: storeResult.FileHandle);

        await AppFactory.GetRequiredService<IRegistryStrategyValidator>()
            .ValidateFileIsNotPresent(key: storeResult.FileHandle);
    }
}