using FluentAssertions;
using Heyer.Storage.API.Validators;
using Microsoft.AspNetCore.Http;
using NSubstitute;

namespace Heyer.Storage.API.Tests.UnitTests.Validators;

[Category("Unit")]
public class FileValidatorTests
{
    private FileValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new FileValidator();
    }

    [Test]
    public void FileValidator_WhenFileLengthIsZero_ShouldReturnValidationError()
    {
        // Arrange
        using var fileStream = File.OpenRead("Utils/TestFiles/test-file.png");
        var file = Substitute.For<IFormFile>();
        file.Length.Returns(0);
        file.FileName.Returns("test-file.png");
        file.OpenReadStream().Returns(fileStream);

        // Act
        var result = _validator.Validate(file);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(x => x.ErrorMessage == "File is empty.");
    }
    
    [Test]
    public void FileValidator_WhenFileLengthIsGreaterThan10MB_ShouldReturnValidationError()
    {
        // Arrange
        using var fileStream = File.OpenRead("Utils/TestFiles/test-file.png");
        var file = Substitute.For<IFormFile>();
        file.Length.Returns(10 * 1024 * 1024 + 1);
        file.FileName.Returns("test-file.png");
        file.OpenReadStream().Returns(fileStream);

        // Act
        var result = _validator.Validate(file);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(x => x.ErrorMessage == "Max file size is 10MB.");
    }
    
    [Test]
    public void FileValidator_WhenFileExtensionIsNotSupported_ShouldReturnValidationError()
    {
        // Arrange
        using var fileStream = File.OpenRead("Utils/TestFiles/test-file.png");
        var file = Substitute.For<IFormFile>();
        file.Length.Returns(1);
        file.FileName.Returns("test-file.txt");
        file.OpenReadStream().Returns(fileStream);

        // Act
        var result = _validator.Validate(file);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(x => x.ErrorMessage == "Invalid file extension.");
    }
    
    [Test]
    public void FileValidator_WhenFileFormatIsNotSupported_ShouldReturnValidationError()
    {
        // Arrange
        using var fileStream = File.OpenRead("Utils/TestFiles/test-file.docx");
        var file = Substitute.For<IFormFile>();
        file.Length.Returns(1);
        file.FileName.Returns("test-file.txt");
        file.OpenReadStream().Returns(fileStream);

        // Act
        var result = _validator.Validate(file);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(x => x.ErrorMessage == "Invalid file format.");
    }
    
    [Test]
    public void FileValidator_WhenFileIsValid_ShouldReturnNoValidationErrors()
    {
        // Arrange
        using var fileStream = File.OpenRead("Utils/TestFiles/test-file.png");
        var file = Substitute.For<IFormFile>();
        file.Length.Returns(1);
        file.FileName.Returns("test-file.png");
        file.OpenReadStream().Returns(fileStream);

        // Act
        var result = _validator.Validate(file);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
}