using FluentValidation;

namespace Heyer.Storage.API.Download;

public class DownloadRequestValidator : AbstractValidator<DownloadRequest>
{
    public DownloadRequestValidator() =>
        RuleFor(x => x.Key)
            .NotEmpty()
            .WithMessage("Key is required.");
}