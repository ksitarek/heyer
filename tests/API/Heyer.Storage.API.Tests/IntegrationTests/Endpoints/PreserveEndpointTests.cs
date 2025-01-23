using System.Net;
using Heyer.Storage.API.Tests.Utils;
using Heyer.Storage.API.Tests.Utils.Validators;
using RestEase;
using Shouldly;

namespace Heyer.Storage.API.Tests.IntegrationTests.Endpoints;

[Category("Integration")]
public class PreserveEndpointTests : StorageApiIntegrationTestsBase
{
    [Test]
    public async Task PreserveEndpoint_WithAlreadyPreservedKey_ReturnsOk()
    {
        // Arrange
        var client = _appFactory.CreateApiClient();
        var storeResult = await client.Store("Utils/TestFiles/test-file.png");

        // Act
        await client.Preserve(storeResult.FileHandle);

        // Assert
        await _appFactory.GetRequiredService<IStorageStrategyValidator>()
            .ValidateFileIsPreserved(storeResult.FileHandle);

        await _appFactory.GetRequiredService<IRegistryStrategyValidator>()
            .ValidateFileIsPreserved(storeResult.FileHandle);
    }

    [Test]
    public async Task PreserveEndpoint_WithInvalidKey_ReturnsNotFound()
    {
        // Arrange
        var client = _appFactory.CreateApiClient();

        // Act
        var action = async () => await client.Preserve("invalid-key");

        // Assert
        var exception = await action.ShouldThrowAsync<ApiException>();
        exception.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task PreserveEndpoint_WithValidKey_ReturnsOk()
    {
        // Arrange
        var client = _appFactory.CreateApiClient();
        var storeResult = await client.Store("Utils/TestFiles/test-file.png");

        // Act
        await client.Preserve(storeResult.FileHandle);

        // Assert
        await _appFactory.GetRequiredService<IStorageStrategyValidator>()
            .ValidateFileIsPreserved(storeResult.FileHandle);

        await _appFactory.GetRequiredService<IRegistryStrategyValidator>()
            .ValidateFileIsPreserved(storeResult.FileHandle);
    }
}