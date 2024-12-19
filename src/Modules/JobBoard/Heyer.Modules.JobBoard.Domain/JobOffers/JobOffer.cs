using FluentResults;
using Heyer.BuildingBlocks.Domain;
using Heyer.Modules.JobBoard.Domain.Candidates;
using Heyer.Modules.JobBoard.Domain.JobOffers.Rules;

namespace Heyer.Modules.JobBoard.Domain.JobOffers;

public class JobOffer : Entity
{
    private HashSet<CandidateId>? _candidates;
    private CompanyDetails _companyDetails = null!;

    private Dictionary<EmploymentType, ContractDetails>? _contractsDetails;
    private string _jobDescription = null!;
    private OfficeLocation? _location;

    private string _offerSummary = null!;

    private DateTimeOffset? _publishedAt;
    private DateTimeOffset? _publishedUntil;
    private RemoteWork _remoteWork;
    private Requirements? _requirements;

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

    public JobOfferId Id { get; } = null!;


    public static JobOffer CreateNew(CompanyDetails companyDetails, string offerSummary, string jobDescription,
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
        EmploymentType employmentType,
        ContractDetails contractDetails)
    {
        var validationResult = ChallengeBusinessRules(
            new JobOfferMustHaveUniqueEmploymentTypes(_contractsDetails, employmentType));

        if (validationResult.IsFailed)
        {
            return validationResult;
        }

        _contractsDetails ??= new Dictionary<EmploymentType, ContractDetails>();

        _contractsDetails.Add(employmentType, contractDetails);

        return Result.Ok();
    }

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
            skills.Select(x => new Skill(x.Key, x.Value)));

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