using Heyer.BuildingBlocks.Tests.Extensions;
using Heyer.Storage.API.Validators;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Shouldly;

namespace Heyer.Storage.API.Tests.UnitTests.Validators;

[Category("Unit")]
public class FileValidatorTests
{
    private FileValidator _validator = null!;

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
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContainSingle(x => x.ErrorMessage == "Invalid file extension.");
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
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContainSingle(x => x.ErrorMessage == "Invalid file format.");
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
        result.IsValid.ShouldBeTrue();
        result.Errors.ShouldBeEmpty();
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
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContainSingle(x => x.ErrorMessage == "Max file size is 10MB.");
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
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContainSingle(x => x.ErrorMessage == "File is empty.");
    }

    [SetUp]
    public void Setup() => _validator = new FileValidator();
}