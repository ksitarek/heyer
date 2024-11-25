using FluentResults;
using FluentValidation.Results;

namespace Heyer.Storage.API.Middleware;

public class ValidationError : Error
{
    public ValidationError(IEnumerable<ValidationFailure> validationFailures)
        : base("Validation failed.")
    {
        foreach (var failure in validationFailures)
        {
            if (Metadata.ContainsKey(failure.PropertyName))
            {
                Metadata[failure.PropertyName] = $"{Metadata[failure.PropertyName]}, {failure.ErrorMessage}";
            }
            else
            {
                Metadata.Add(failure.PropertyName, failure.ErrorMessage);
            }
        }
    }
}