using FluentValidation;
using Heyer.Storage.API.Client.PublishedLanguage;

namespace Heyer.Storage.API.Store;

public class StoreRequestValidator : AbstractValidator<StoreRequest>
{
    public StoreRequestValidator(IValidator<IFormFile> fileValidator) =>
        RuleFor(x => x.File)
            .NotNull()
            .WithMessage("File is required.")
            .SetValidator(fileValidator);
}