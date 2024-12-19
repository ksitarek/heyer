using FluentAssertions;
using Heyer.Storage.API.Client.PublishedLanguage;
using Heyer.Storage.API.Preserve;

namespace Heyer.Storage.API.Tests.UnitTests.Preserve;

[Category("Unit")]
public class PreserveRequestValidatorTests
{
    private PreserveRequestValidator _validator;

    [SetUp]
    public void SetUp() => _validator = new PreserveRequestValidator();

    [Test]
    public void Validate_WhenKeyIsMissing_ShouldReturnError()
    {
        // Arrange
        var request = new PreserveRequest(string.Empty);

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(x => x.ErrorMessage == "Key is required.");
    }

    [Test]
    public void Validate_WhenKeyIsPresent_ShouldNotReturnError()
    {
        // Arrange
        var request = new PreserveRequest("key");

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
}