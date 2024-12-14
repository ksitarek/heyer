using FluentAssertions;
using FluentValidation;
using FluentValidation.TestHelper;
using Heyer.Storage.API.Client.PublishedLanguage;
using Heyer.Storage.API.Store;
using Microsoft.AspNetCore.Http;
using NSubstitute;

namespace Heyer.Storage.API.Tests.UnitTests.Store;

[Category("Unit")]
public class StoreRequestValidatorTests
{
    private StoreRequestValidator _validator;
    private IValidator<IFormFile> _fileValidator;

    [SetUp]
    public void Setup()
    {
        _fileValidator = Substitute.For<IValidator<IFormFile>>();
        _validator = new StoreRequestValidator(_fileValidator);
    }

    [Test]
    public void ShouldHaveErrorWhenFileIsNull()
    {
        // Arrange
        var request = new StoreRequest(default!);

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.ErrorMessage == "File is required.");
    }

    [Test]
    public void ShouldHaveErrorWhenFileIsInvalid()
    {
        // Arrange

        // Act

        // Assert
        _validator.ShouldHaveChildValidator(x => x.File, typeof(IValidator<IFormFile>));
    }

    [Test]
    public void ShouldNotHaveErrorWhenFileIsValid()
    {
        // Arrange
        var request = new StoreRequest(Substitute.For<IFormFile>());

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}