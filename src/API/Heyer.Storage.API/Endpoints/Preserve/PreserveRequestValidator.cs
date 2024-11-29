using FluentValidation;
using Heyer.Storage.API.Client.PublishedLanguage;

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