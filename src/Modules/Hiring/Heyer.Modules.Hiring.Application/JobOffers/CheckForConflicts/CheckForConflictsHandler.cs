using FluentResults;
using Heyer.BuildingBlocks.Application.Results;
using Heyer.BuildingBlocks.Infrastructure.Messaging;
using Heyer.Modules.Hiring.Domain.JobOffers;

namespace Heyer.Modules.Hiring.Application.JobOffers.CheckForConflicts;

public class CheckForConflictsHandler : IQueryHandler<CheckForConflicts, bool>
{
    private readonly IJobOffersRepository _repository;

    public CheckForConflictsHandler(IJobOffersRepository repository) => _repository = repository;

    public async Task<Result<bool>> Handle(CheckForConflicts request, CancellationToken cancellationToken)
    {
        var subjectOffer = await _repository.GetJobOfferById(request.Id, cancellationToken);

        if (subjectOffer is null)
        {
            return new NotFoundError();
        }

        return await _repository.CheckForConflicts(subjectOffer, cancellationToken);
    }
}