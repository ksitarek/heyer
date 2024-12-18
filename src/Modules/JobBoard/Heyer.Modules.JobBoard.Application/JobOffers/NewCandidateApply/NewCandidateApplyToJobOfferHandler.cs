using FluentResults;
using Heyer.BuildingBlocks.Application.Results;
using Heyer.BuildingBlocks.Infrastructure.Messaging;
using Heyer.Modules.JobBoard.Domain.Candidates;
using Heyer.Modules.JobBoard.Domain.JobOffers;

namespace Heyer.Modules.JobBoard.Application.JobOffers.NewCandidateApply;

public class NewCandidateApplyToJobOfferHandler : ICommandHandler<NewCandidateApplyToJobOffer>
{
    private readonly IJobOffersRepository _jobOffersRepository;
    private readonly ICandidatesRepository _candidatesRepository;

    public NewCandidateApplyToJobOfferHandler(
        IJobOffersRepository jobOffersRepository,
        ICandidatesRepository candidatesRepository)
    {
        _jobOffersRepository = jobOffersRepository;
        _candidatesRepository = candidatesRepository;
    }
    
    public async Task<Result> Handle(NewCandidateApplyToJobOffer request, CancellationToken cancellationToken)
    {
        var jobOffer = await _jobOffersRepository.GetJobOfferById(request.JobOfferId, cancellationToken);
        if (jobOffer is null)
        {
            return new NotFoundError();
        }

        var createCandidateResult = await CreateCandidate(request, cancellationToken);

        if (createCandidateResult.IsSuccess)
        {
            jobOffer.AddCandidate(createCandidateResult.Value.Id);

            return Result.Ok();
        }

        return Result.Fail(createCandidateResult.Errors);
    }

    private async Task<Result<Candidate>> CreateCandidate(NewCandidateApplyToJobOffer request, CancellationToken cancellationToken)
    {
        var candidate = Candidate.Create(
            request.FirstName,
            request.LastName,
            new Email(request.Email),
            new ResumeKey(request.ResumeKey),
            request.IncludeInCandidatePool,
            request.Attributes);

        var result = await _candidatesRepository.AddCandidate(candidate, cancellationToken);

        return result.IsSuccess
            ? candidate
            : result;
    }
}