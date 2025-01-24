using FluentResults;
using Heyer.BuildingBlocks.Application.Results;
using Heyer.BuildingBlocks.Infrastructure.Messaging;
using Heyer.Modules.Hiring.Domain.JobOffers;
using Heyer.Modules.Hiring.PublishedLanguage.DTOs;

namespace Heyer.Modules.Hiring.Application.JobOffers.SetRequirements;

public class SetRequirementsHandler : ICommandHandler<SetRequirements>
{
    private readonly IJobOffersRepository _jobOffersRepository;

    public SetRequirementsHandler(IJobOffersRepository jobOffersRepository) =>
        _jobOffersRepository = jobOffersRepository;

    public async Task<Result> Handle(SetRequirements request, CancellationToken cancellationToken)
    {
        var jobOffer = await _jobOffersRepository.GetJobOfferById(request.Id, cancellationToken);

        if (jobOffer is null)
        {
            return new NotFoundError();
        }

        var experienceLevel = request.Requirements.ExperienceLevel;
        var skills = request.Requirements.Skills?.ToDictionary(x => x.Label, x => x.Level)
                     ?? new Dictionary<string, SkillLevel>();

        return jobOffer.SetRequirements(experienceLevel, skills);
    }
}