using FluentAssertions;
using Heyer.Storage.API.Client.PublishedLanguage;
using Heyer.Storage.API.Delete;

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
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(x => x.ErrorMessage == "Key is required.");
    }

    [Test]
    public void DeleteRequestValidator_WhenIdIsNotEmpty_ShouldNotReturnValidationError()
    {
        // Arrange
        var request = new DeleteRequest("key");

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [SetUp]
    public void Setup() => _validator = new DeleteRequestValidator();
}