using FluentResults;
using Heyer.BuildingBlocks.Domain;
using Heyer.Modules.Hiring.PublishedLanguage;
using Heyer.Modules.JobBoard.Domain.JobOffers.Events;
using Heyer.Modules.JobBoard.Domain.JobOffers.Rules;
using SkillLevel = Heyer.Modules.Hiring.PublishedLanguage.SkillLevel;

namespace Heyer.Modules.JobBoard.Domain.JobOffers;

public class PublishedJobOffer : Entity
{
    // For EF Core
    private PublishedJobOffer()
    {
    }

    private PublishedJobOffer(CompanyDetails companyDetails,
                              string offerSummary,
                              string jobDescription,
                              RemoteWork remoteWork)
    {
        Id = PublishedJobOfferId.CreateNew();

        CompanyDetails = companyDetails;
        OfferSummary = offerSummary;
        JobDescription = jobDescription;
        RemoteWork = remoteWork;

        AddDomainEvent(new JobOfferPublished(Id));
    }

    public CompanyDetails CompanyDetails { get; private set; } = null!;

    public List<ContractDetails>? ContractsDetails { get; private set; }

    public PublishedJobOfferId Id { get; private set; } = null!;
    public string JobDescription { get; private set; } = null!;
    public OfficeLocation? Location { get; private set; }

    public string OfferSummary { get; private set; } = null!;

    public DateTimeOffset? PublishedUntil { get; private set; }
    public RemoteWork RemoteWork { get; private set; }
    public Requirements? Requirements { get; private set; }


    public static PublishedJobOffer CreateNew(CompanyDetails companyDetails,
                                              string offerSummary,
                                              string jobDescription,
                                              RemoteWork remoteWork) =>
        new(companyDetails, offerSummary, jobDescription, remoteWork);

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
            new JobOfferMustBePublishedToTakeDown(PublishedUntil));

        if (validationResult.IsFailed)
        {
            return validationResult;
        }

        PublishedUntil = DateTimeOffset.UtcNow;

        AddDomainEvent(new JobOfferTakenDown(Id, PublishedUntil));

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