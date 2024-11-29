using FluentValidation;
using Heyer.Storage.API.Client.PublishedLanguage;

namespace Heyer.Storage.API.Endpoints.Download;

public class DownloadRequestValidator : AbstractValidator<DownloadRequest>
{
    public DownloadRequestValidator()
    {
        RuleFor(x => x.Key)
            .NotEmpty()
            .WithMessage("Key is required.");
    }
}