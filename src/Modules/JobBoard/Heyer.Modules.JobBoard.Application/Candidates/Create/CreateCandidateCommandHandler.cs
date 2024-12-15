using FluentResults;
using Heyer.BuildingBlocks.Infrastructure;
using Heyer.BuildingBlocks.Infrastructure.Messaging;
using Heyer.Modules.JobBoard.Domain.Candidates;

namespace Heyer.Modules.JobBoard.Application.Candidates.Create;

public class CreateCandidateCommandHandler : ICommandHandler<CreateCandidate>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICandidateRepository _candidateRepository;

    public CreateCandidateCommandHandler(IDateTimeProvider dateTimeProvider, ICandidateRepository candidateRepository)
    {
        _dateTimeProvider = dateTimeProvider;
        _candidateRepository = candidateRepository;
    }

    public async Task<Result> Handle(CreateCandidate request, CancellationToken cancellationToken)
    {
        var candidate = Candidate.CreateNew(
            request.FirstName,
            request.LastName,
            request.Email,
            request.ResumeKey,
            request.IncludeInCandidatePool,
            _dateTimeProvider.UtcNow(),
            request.Attributes);

        await _candidateRepository.AddAsync(candidate, cancellationToken);

        return Result.Ok();
    }
}