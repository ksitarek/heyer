using FluentValidation;
using Heyer.Storage.API.Client.PublishedLanguage;

namespace Heyer.Storage.API.Endpoints.Delete;

public class DeleteRequestValidator : AbstractValidator<DeleteRequest>
{
    public DeleteRequestValidator()
    {
        RuleFor(x => x.Key)
            .NotEmpty()
            .WithMessage("Key is required.");
    }
}