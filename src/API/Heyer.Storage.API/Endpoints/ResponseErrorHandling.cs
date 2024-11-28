using FluentResults;
using Heyer.Storage.API.Middleware;

namespace Heyer.Storage.API.Endpoints;

public static class ResponseErrorHandling
{
    public static IResult Handle(IResultBase response)
    {
        var error = response.Errors[0];

        return error switch
        {
            NotFoundError => Results.NotFound(),
            ValidationError => HandleValidationError(response.Errors),

            _ => Results.StatusCode(StatusCodes.Status500InternalServerError)
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

        return Results.ValidationProblem(validationProblems.ToDictionary(x => x.Key, x => x.Value.ToArray()));
    }
}