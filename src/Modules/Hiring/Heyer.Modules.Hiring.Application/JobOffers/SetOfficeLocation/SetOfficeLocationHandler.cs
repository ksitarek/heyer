using FluentResults;
using Heyer.BuildingBlocks.Application.Results;
using Heyer.BuildingBlocks.Infrastructure.Messaging;
using Heyer.Modules.Hiring.Domain.JobOffers;

namespace Heyer.Modules.Hiring.Application.JobOffers.SetOfficeLocation;

public class SetOfficeLocationHandler : ICommandHandler<SetOfficeLocation>
{
    private readonly IJobOffersRepository _jobOffersRepository;

    public SetOfficeLocationHandler(IJobOffersRepository jobOffersRepository) =>
        _jobOffersRepository = jobOffersRepository;

    public async Task<Result> Handle(SetOfficeLocation request, CancellationToken cancellationToken)
    {
        var jobOffer = await _jobOffersRepository.GetJobOfferById(request.Id, cancellationToken);

        if (jobOffer is null)
        {
            return new NotFoundError();
        }

        return jobOffer.SetOfficeLocation(request.Location);
    }
}