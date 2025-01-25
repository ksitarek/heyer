using FluentValidation;
using FluentValidation.TestHelper;
using Heyer.BuildingBlocks.Tests.Extensions;
using Heyer.Storage.API.Store;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Shouldly;

namespace Heyer.Storage.API.Tests.UnitTests.Store;

[Category("Unit")]
public class StoreRequestValidatorTests
{
    private IValidator<IFormFile> _fileValidator = null!;
    private StoreRequestValidator _validator = null!;

    [SetUp]
    public void Setup()
    {
        _fileValidator = Substitute.For<IValidator<IFormFile>>();
        _validator = new StoreRequestValidator(_fileValidator);
    }

    [Test]
    public void ShouldHaveErrorWhenFileIsInvalid() =>
        // Arrange
        // Act
        // Assert
        _validator.ShouldHaveChildValidator(x => x.File, typeof(IValidator<IFormFile>));

    [Test]
    public void ShouldHaveErrorWhenFileIsNull()
    {
        // Arrange
        var request = new StoreRequest(default!);

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContainSingle(e => e.ErrorMessage == "File is required.");
    }

    [Test]
    public void ShouldNotHaveErrorWhenFileIsValid()
    {
        // Arrange
        var request = new StoreRequest(Substitute.For<IFormFile>());

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.ShouldBeTrue();
    }
}