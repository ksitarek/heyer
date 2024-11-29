using FluentValidation;
using Heyer.Storage.API.Client.PublishedLanguage;
using Heyer.Storage.API.Validators;

namespace Heyer.Storage.API.Endpoints.Store;

public class StoreRequestValidator : AbstractValidator<StoreRequest>
{
    public StoreRequestValidator()
    {
        RuleFor(x => x.File)
            .NotNull()
            .WithMessage("File is required.")
            .SetValidator(new FileValidator());
    }
}