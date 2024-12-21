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

        CompanyDetails = companyDetails;
        OfferSummary = offerSummary;
        JobDescription = jobDescription;
        RemoteWork = remoteWork;

        AddDomainEvent(new JobOfferCreated(Id));
    }

    public HashSet<CandidateId>? Candidates { get; private set; }
    public CompanyDetails CompanyDetails { get; private set; } = null!;

    public List<ContractDetails>? ContractsDetails { get; private set; }

    public JobOfferId Id { get; private set; } = null!;
    public string JobDescription { get; private set; } = null!;
    public OfficeLocation? Location { get; private set; }

    public string OfferSummary { get; private set; } = null!;

    public DateTimeOffset? PublishedAt { get; private set; }
    public DateTimeOffset? PublishedUntil { get; private set; }
    public RemoteWork RemoteWork { get; private set; }
    public Requirements? Requirements { get; private set; }


    public static JobOffer CreateNew(CompanyDetails companyDetails,
                                     string offerSummary,
                                     string jobDescription,
                                     RemoteWork remoteWork) =>
        new(companyDetails, offerSummary, jobDescription, remoteWork);

    public Result AddCandidate(
        CandidateId candidateId)
    {
        var validationResult = ChallengeBusinessRules(
            new CandidateCanApplyOnlyOnce(Candidates, candidateId));

        if (validationResult.IsFailed)
        {
            return validationResult;
        }

        Candidates ??= new HashSet<CandidateId>();

        Candidates.Add(candidateId);

        AddDomainEvent(new CandidateApplied(Id, candidateId));

        return Result.Ok();
    }

    public Result AddContractDetails(
        ContractDetails newContractDetails)
    {
        var validationResult = ChallengeBusinessRules(
            new JobOfferMustHaveUniqueEmploymentTypes(ContractsDetails, newContractDetails.EmploymentType));

        if (validationResult.IsFailed)
        {
            return validationResult;
        }

        ContractsDetails ??= new List<ContractDetails>();

        ContractsDetails.Add(newContractDetails);

        return Result.Ok();
    }

    public Result Publish(DateTimeOffset? publishedUntil)
    {
        var validationResult = ChallengeBusinessRules(
            new PublishedUntilMustNotBeInPast(publishedUntil),
            new JobOfferMustHaveRequirementsWhenPublishing(Requirements),
            new JobOfferMustHaveLocationWhenPublishing(Location));

        if (validationResult.IsFailed)
        {
            return validationResult;
        }

        PublishedAt = DateTimeOffset.UtcNow;
        PublishedUntil = publishedUntil;

        AddDomainEvent(new JobOfferPublished(Id));

        return Result.Ok();
    }


    public Result SetOfficeLocation(OfficeLocation location)
    {
        Location = location;

        return Result.Ok();
    }

    public Result SetRequirements(ExperienceLevel experienceLevel, IDictionary<string, SkillLevel> skills)
    {
        Requirements = new Requirements(
            experienceLevel,
            skills.Select(x => new Skill(x.Key, x.Value))
                .ToList());

        return Result.Ok();
    }

    public Result TakeDown()
    {
        var validationResult = ChallengeBusinessRules(
            new JobOfferMustBePublishedToTakeDown(PublishedAt));

        if (validationResult.IsFailed)
        {
            return validationResult;
        }

        PublishedAt = null;
        PublishedUntil = null;

        AddDomainEvent(new JobOfferTakenDown(Id));

        return Result.Ok();
    }

    public Result UpdateDescription(string offerSummary, string jobDescription)
    {
        OfferSummary = offerSummary;
        JobDescription = jobDescription;

        AddDomainEvent(new JobOfferDescriptionUpdated(Id));

        return Result.Ok();
    }
}