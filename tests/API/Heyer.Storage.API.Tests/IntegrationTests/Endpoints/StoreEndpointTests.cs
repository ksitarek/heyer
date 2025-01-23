using System.Net;
using Heyer.BuildingBlocks.Json;
using Heyer.Storage.API.Tests.Utils;
using Heyer.Storage.API.Tests.Utils.Validators;
using Microsoft.AspNetCore.Mvc;
using RestEase;
using Shouldly;

namespace Heyer.Storage.API.Tests.IntegrationTests.Endpoints;

[Category("Integration")]
public class StoreEndpointTests : StorageApiIntegrationTestsBase
{
    [Test]
    public async Task StoreEndpoint_WithInvalidFile_ReturnsOkWithFileHandle()
    {
        // Arrange
        var client = _appFactory.CreateApiClient();

        // Act
        var action = async () => await client.Store("Utils/TestFiles/test-file.docx");

        // Assert
        var exception = await action.ShouldThrowAsync<ApiException>();

        exception.StatusCode.ShouldBe(HttpStatusCode.BadRequest);


        var validationDetails = exception.Content!.Deserialize<ValidationProblemDetails>()!;

        validationDetails.ShouldNotBeNull();
        validationDetails.Errors.Count.ShouldBe(2);
        validationDetails.Errors.Keys.ShouldContain("File.FileName", "File");
        validationDetails.Errors["File"].ShouldContain("Invalid file format.");
        validationDetails.Errors["File.FileName"].ShouldContain("Invalid file extension.");
    }

    [Test]
    public async Task StoreEndpoint_WithValidFile_ReturnsOkWithFileHandle()
    {
        // Arrange
        var client = _appFactory.CreateApiClient();

        // Act
        var storeResult = await client.Store("Utils/TestFiles/test-file.png");

        // Assert
        storeResult.ShouldNotBeNull();
        storeResult.FileHandle.ShouldNotBeNull();

        await _appFactory.GetRequiredService<IStorageStrategyValidator>()
            .ValidateFileIsPresent(storeResult.FileHandle);

        await _appFactory.GetRequiredService<IRegistryStrategyValidator>()
            .ValidateFilePropertiesAsync(
                storeResult.FileHandle,
                "test-file.png",
                "image/png",
                2620);
    }
}