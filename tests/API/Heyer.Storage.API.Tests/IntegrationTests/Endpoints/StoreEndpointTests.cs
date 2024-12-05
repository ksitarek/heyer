using System.Net;
using System.Text.Json;
using FluentAssertions;
using Heyer.Storage.API.Tests.Utils;
using Heyer.Storage.API.Tests.Utils.Validators;
using Microsoft.AspNetCore.Mvc;
using RestEase;

namespace Heyer.Storage.API.Tests.IntegrationTests.Endpoints;

[Category("Integration")]
public class StoreEndpointTests : IntegrationTestsBase
{
    [Test]
    public async Task StoreEndpoint_WithValidFile_ReturnsOkWithFileHandle()
    {
        // Arrange
        var client = AppFactory.CreateApiClient();

        // Act
        var storeResult = await client.Store("Utils/TestFiles/test-file.png");

        // Assert
        storeResult.Should().NotBeNull();
        storeResult.FileHandle.Should().NotBeNull();
        
        await AppFactory.GetRequiredService<IStorageStrategyValidator>()
            .ValidateFileIsPresent(key: storeResult.FileHandle);

        await AppFactory.GetRequiredService<IRegistryStrategyValidator>()
            .ValidateFilePropertiesAsync(
                key: storeResult.FileHandle,
                expectedFileName: "test-file.png",
                expectedContentType: "image/png",
                expectedSize: 2620);
    }

    [Test]
    public async Task StoreEndpoint_WithInvalidFile_ReturnsOkWithFileHandle()
    {
        // Arrange
        var client = AppFactory.CreateApiClient();

        // Act
        var action = async () => await client.Store("Utils/TestFiles/test-file.docx");

        // Assert
        var exception = await action.Should().ThrowAsync<ApiException>()
            .Where(e => e.StatusCode == HttpStatusCode.BadRequest);

        var validationDetails = JsonSerializer.Deserialize<ValidationProblemDetails>(exception.Which.Content!)!;
        
        validationDetails.Should().NotBeNull();
        validationDetails.Errors.Should().HaveCount(2).And.ContainKeys("File.FileName", "File");
        validationDetails.Errors["File"].Should().Contain("Invalid file format.");
        validationDetails.Errors["File.FileName"].Should().Contain("Invalid file extension.");
    }
}