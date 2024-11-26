using FluentValidation;

namespace Heyer.Storage.API.Endpoints.Preserve;

public class PreserveRequestValidator : AbstractValidator<PreserveRequest>
{
    public PreserveRequestValidator()
    {
        RuleFor(x => x.Key)
            .NotEmpty()
            .WithMessage("Key is required.");
    }
}