using System.Net;
using FluentAssertions;
using Heyer.Storage.API.Tests.Utils;
using Heyer.Storage.API.Tests.Utils.Validators;
using RestEase;

namespace Heyer.Storage.API.Tests.IntegrationTests.Endpoints;

[Category("Integration")]
public class PreserveEndpointTests : IntegrationTestsBase
{
    [Test]
    public async Task PreserveEndpoint_WithValidKey_ReturnsOk()
    {
        // Arrange
        var client = AppFactory.CreateApiClient();
        var storeResult = await client.Store("Utils/TestFiles/test-file.png");
        
        // Act
        await client.Preserve(storeResult.FileHandle);
        
        // Assert
        await AppFactory.GetRequiredService<IStorageStrategyValidator>()
            .ValidateFileIsPreserved(key: storeResult.FileHandle);
        
        await AppFactory.GetRequiredService<IRegistryStrategyValidator>()
            .ValidateFileIsPreserved(key: storeResult.FileHandle);
        
    }
    
    [Test]
    public async Task PreserveEndpoint_WithInvalidKey_ReturnsNotFound()
    {
        // Arrange
        var client = AppFactory.CreateApiClient();
        
        // Act
        var action = async () => await client.Preserve("invalid-key");
        
        // Assert
        (await action.Should().ThrowAsync<ApiException>()).And.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Test]
    public async Task PreserveEndpoint_WithAlreadyPreservedKey_ReturnsOk()
    {
        // Arrange
        var client = AppFactory.CreateApiClient();
        var storeResult = await client.Store("Utils/TestFiles/test-file.png");
        
        // Act
        await client.Preserve(storeResult.FileHandle);
        
        // Assert
        await AppFactory.GetRequiredService<IStorageStrategyValidator>()
            .ValidateFileIsPreserved(key: storeResult.FileHandle);
        
        await AppFactory.GetRequiredService<IRegistryStrategyValidator>()
            .ValidateFileIsPreserved(key: storeResult.FileHandle);
    }
}