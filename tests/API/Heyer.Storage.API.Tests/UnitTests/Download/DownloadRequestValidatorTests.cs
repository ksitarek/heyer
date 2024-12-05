using FluentAssertions;
using Heyer.Storage.API.Client.PublishedLanguage;
using Heyer.Storage.API.Download;

namespace Heyer.Storage.API.Tests.UnitTests.Download;

[Category("Unit")]
public class DownloadRequestValidatorTests
{
    private DownloadRequestValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new DownloadRequestValidator();
    }
    
    [Test]
    public void Validate_WhenKeyIsMissing_ShouldReturnError()
    {
        // Arrange
        var request = new DownloadRequest(string.Empty);
        
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
        var request = new DownloadRequest("key");
        
        // Act
        var result = _validator.Validate(request);
        
        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
}