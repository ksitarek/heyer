using FluentResults;
using Heyer.BuildingBlocks.Domain;
using Microsoft.AspNetCore.Http;

namespace Heyer.BuildingBlocks.Application.Results;

public static class ResponseErrorHandling
{
    public static IResult Handle(IResultBase response)
    {
        var error = response.Errors[0];

        return error switch
        {
            NotFoundError => Microsoft.AspNetCore.Http.Results.NotFound(),
            BusinessRuleViolationError => Microsoft.AspNetCore.Http.Results.BadRequest(),
            ValidationError => HandleValidationError(response.Errors),

            _ => Microsoft.AspNetCore.Http.Results.StatusCode(StatusCodes.Status500InternalServerError)
        };
    }

    private static IResult HandleValidationError(List<IError> errors)
    {
        var validationProblems = new Dictionary<string, List<string>>();
        var validationErrors = errors.Where(x => x is ValidationError);

        foreach (var metadata in validationErrors.SelectMany(x => x.Metadata))
        {
            if (validationProblems.ContainsKey(metadata.Key))
            {
                validationProblems[metadata.Key].Add(metadata.Key);
            }
            else
            {
                var metadataValues = metadata.Value.ToString()!
                    .Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .ToList();

                validationProblems.Add(metadata.Key, metadataValues);
            }
        }

        return Microsoft.AspNetCore.Http.Results.ValidationProblem(
            validationProblems.ToDictionary(x => x.Key, x => x.Value.ToArray()));
    }
}