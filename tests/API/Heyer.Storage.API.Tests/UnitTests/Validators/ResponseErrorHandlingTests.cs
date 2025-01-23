using FluentValidation.Results;
using Heyer.BuildingBlocks.Application.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Shouldly;
using Result = FluentResults.Result;

namespace Heyer.Storage.API.Tests.UnitTests.Validators;

[Category("Unit")]
public class ResponseErrorHandlingTests
{
    [Test]
    public void Handle_ShouldMapNotFoundErrorToHttpNotFound()
    {
        // Arrange
        var error = Result.Fail(new NotFoundError());

        // Act
        var result = ResponseErrorHandling.Handle(error);

        // Assert
        result.ShouldBeSameAs(Results.NotFound());
    }

    [Test]
    public void Handle_ShouldMapValidationErrorToValidationProblem()
    {
        // Arrange
        var error = Result.Fail(new ValidationError(new[]
        {
            new ValidationFailure(
                "Test Property",
                "Test Error Message #1"),
            new ValidationFailure(
                "Test Property",
                "Test Error Message #2"),
            new ValidationFailure(
                "Another Test Property",
                "Test Error Message #3")
        }));

        // Act
        var result = ResponseErrorHandling.Handle(error);

        // Assert
        var errors = new List<KeyValuePair<string, string[]>>
        {
            new("Test Property",
            [
                "Test Error Message #1",
                "Test Error Message #2"
            ]),
            new("Another Test Property", ["Test Error Message #3"])
        };

        result.ShouldBeOfType<ProblemHttpResult>();

        var problemDetails = ((ProblemHttpResult)result).ProblemDetails;
        problemDetails.ShouldBeOfType<HttpValidationProblemDetails>();

        var httpValidationProblemDetails = (HttpValidationProblemDetails)problemDetails;

        foreach (var e in errors)
        {
            httpValidationProblemDetails.Errors.ShouldContainKey(e.Key);
            httpValidationProblemDetails.Errors[e.Key].ShouldBeEquivalentTo(e.Value);
        }
    }

    [Test]
    public void Handle_WillMapResultFailedToInternalServerError()
    {
        // Arrange
        var fail = Result.Fail("");

        // Act
        var result = ResponseErrorHandling.Handle(fail);

        // Assert
        result.ShouldBeSameAs(Results.StatusCode(StatusCodes.Status500InternalServerError));
    }
}