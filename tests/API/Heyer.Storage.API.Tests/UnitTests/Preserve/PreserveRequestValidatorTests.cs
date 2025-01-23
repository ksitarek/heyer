using Heyer.BuildingBlocks.Tests.Extensions;
using Heyer.Storage.API.Preserve;
using Shouldly;

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
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContainSingle(x => x.ErrorMessage == "Key is required.");
    }

    [Test]
    public void Validate_WhenKeyIsPresent_ShouldNotReturnError()
    {
        // Arrange
        var request = new PreserveRequest("key");

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.ShouldBeTrue();
        result.Errors.ShouldBeEmpty();
    }
}