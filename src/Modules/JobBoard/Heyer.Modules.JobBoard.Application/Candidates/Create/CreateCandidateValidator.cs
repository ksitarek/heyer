using FluentValidation;

namespace Heyer.Modules.JobBoard.Application.Candidates.Create;

public class CreateCandidateValidator : AbstractValidator<CreateCandidate>
{
    public CreateCandidateValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.ResumeKey).NotEmpty();
    }
}