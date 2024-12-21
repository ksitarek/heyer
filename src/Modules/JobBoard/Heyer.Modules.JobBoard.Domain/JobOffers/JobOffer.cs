using FluentResults;
using Heyer.BuildingBlocks.Domain;
using Heyer.Modules.JobBoard.Domain.Candidates;
using Heyer.Modules.JobBoard.Domain.JobOffers.Rules;

namespace Heyer.Modules.JobBoard.Domain.JobOffers;

public class JobOffer : Entity
{
    // For EF Core
    private JobOffer()
    {
    }

    private JobOffer(CompanyDetails companyDetails, string offerSummary, string jobDescription, RemoteWork remoteWork)
    {
        Id = JobOfferId.CreateNew();

        _companyDetails = companyDetails;
        _offerSummary = offerSummary;
        _jobDescription = jobDescription;
        _remoteWork = remoteWork;

        AddDomainEvent(new JobOfferCreated(Id));
    }

    public HashSet<CandidateId>? _candidates { get; private set; }
    public CompanyDetails _companyDetails { get; private set; } = null!;

    public List<ContractDetails>? _contractsDetails { get; private set; }
    public string _jobDescription { get; private set; } = null!;
    public OfficeLocation? _location { get; private set; }

    public string _offerSummary { get; private set; } = null!;

    public DateTimeOffset? _publishedAt { get; private set; }
    public DateTimeOffset? _publishedUntil { get; private set; }
    public RemoteWork _remoteWork { get; private set; }
    public Requirements? _requirements { get; private set; }

    public JobOfferId Id { get; private set; } = null!;


    public static JobOffer CreateNew(CompanyDetails companyDetails,
                                     string offerSummary,
                                     string jobDescription,
                                     RemoteWork remoteWork) =>
        new(companyDetails, offerSummary, jobDescription, remoteWork);

    public Result AddCandidate(
        CandidateId candidateId)
    {
        var validationResult = ChallengeBusinessRules(
            new CandidateCanApplyOnlyOnce(_candidates, candidateId));

        if (validationResult.IsFailed)
        {
            return validationResult;
        }

        _candidates ??= new HashSet<CandidateId>();

        _candidates.Add(candidateId);

        AddDomainEvent(new CandidateApplied(Id, candidateId));

        return Result.Ok();
    }

    public Result AddContractDetails(
        ContractDetails newContractDetails)
    {
        var validationResult = ChallengeBusinessRules(
            new JobOfferMustHaveUniqueEmploymentTypes(_contractsDetails, newContractDetails.EmploymentType));

        if (validationResult.IsFailed)
        {
            return validationResult;
        }

        _contractsDetails ??= new List<ContractDetails>();

        _contractsDetails.Add(newContractDetails);

        return Result.Ok();
    }

    public string GetJobDescription() => _jobDescription;

    public string GetOfferSummary() => _offerSummary;

    public Result Publish(DateTimeOffset? publishedUntil)
    {
        var validationResult = ChallengeBusinessRules(
            new PublishedUntilMustNotBeInPast(publishedUntil),
            new JobOfferMustHaveRequirementsWhenPublishing(_requirements),
            new JobOfferMustHaveLocationWhenPublishing(_location));

        if (validationResult.IsFailed)
        {
            return validationResult;
        }

        _publishedAt = DateTimeOffset.UtcNow;
        _publishedUntil = publishedUntil;

        AddDomainEvent(new JobOfferPublished(Id));

        return Result.Ok();
    }


    public Result SetOfficeLocation(OfficeLocation location)
    {
        _location = location;

        return Result.Ok();
    }

    public Result SetRequirements(ExperienceLevel experienceLevel, IDictionary<string, SkillLevel> skills)
    {
        _requirements = new Requirements(
            experienceLevel,
            skills.Select(x => new Skill(x.Key, x.Value))
                .ToList());

        return Result.Ok();
    }

    public Result TakeDown()
    {
        var validationResult = ChallengeBusinessRules(
            new JobOfferMustBePublishedToTakeDown(_publishedAt));

        if (validationResult.IsFailed)
        {
            return validationResult;
        }

        _publishedAt = null;
        _publishedUntil = null;

        AddDomainEvent(new JobOfferTakenDown(Id));

        return Result.Ok();
    }

    public Result UpdateDescription(string offerSummary, string jobDescription)
    {
        _offerSummary = offerSummary;
        _jobDescription = jobDescription;

        AddDomainEvent(new JobOfferDescriptionUpdated(Id));

        return Result.Ok();
    }
}