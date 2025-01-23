using Heyer.BuildingBlocks.Tests.Extensions;
using Heyer.Storage.API.Client.PublishedLanguage;
using Heyer.Storage.API.Delete;
using Shouldly;

namespace Heyer.Storage.API.Tests.UnitTests.Delete;

[Category("Unit")]
public class DeleteRequestValidatorTests
{
    private DeleteRequestValidator _validator;

    [Test]
    public void DeleteRequestValidator_WhenIdIsEmpty_ShouldReturnValidationError()
    {
        // Arrange
        var request = new DeleteRequest(string.Empty);

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContainSingle(x => x.ErrorMessage == "Key is required.");
    }

    [Test]
    public void DeleteRequestValidator_WhenIdIsNotEmpty_ShouldNotReturnValidationError()
    {
        // Arrange
        var request = new DeleteRequest("key");

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.ShouldBeTrue();
        result.Errors.ShouldBeEmpty();
    }

    [SetUp]
    public void Setup() => _validator = new DeleteRequestValidator();
}