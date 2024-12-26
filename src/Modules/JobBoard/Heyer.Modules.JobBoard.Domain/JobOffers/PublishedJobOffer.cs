using FluentResults;
using Heyer.BuildingBlocks.Domain;
using Heyer.Modules.Hiring.PublishedLanguage.DTOs;
using Heyer.Modules.JobBoard.Domain.JobOffers.Events;
using Heyer.Modules.JobBoard.Domain.JobOffers.Rules;

namespace Heyer.Modules.JobBoard.Domain.JobOffers;

public class PublishedJobOffer : Entity
{
    // For EF Core
    private PublishedJobOffer()
    {
    }

    private PublishedJobOffer(PublishedJobOfferId id,
                              CompanyDetails companyDetails,
                              string offerSummary,
                              string jobDescription,
                              RemoteWork remoteWork,
                              List<ContractDetails> contractsDetails,
                              OfficeLocation location,
                              DateTimeOffset? publishedUntil,
                              Requirements requirements)
    {
        Id = id;
        CompanyDetails = companyDetails;
        OfferSummary = offerSummary;
        JobDescription = jobDescription;
        RemoteWork = remoteWork;
        ContractsDetails = contractsDetails;
        Location = location;
        PublishedUntil = publishedUntil;
        Requirements = requirements;


        AddDomainEvent(new JobOfferPublished(Id));
    }

    public CompanyDetails CompanyDetails { get; private set; } = null!;

    public List<ContractDetails> ContractsDetails { get; private set; } = new();

    public PublishedJobOfferId Id { get; private set; } = null!;
    public string JobDescription { get; private set; } = null!;
    public OfficeLocation Location { get; private set; } = null!;

    public string OfferSummary { get; private set; } = null!;

    public DateTimeOffset? PublishedUntil { get; private set; }
    public RemoteWork RemoteWork { get; private set; }
    public Requirements Requirements { get; private set; } = null!;


    public static PublishedJobOffer CreateNew(
        PublishedJobOfferId id,
        CompanyDetails companyDetails,
        string offerSummary,
        string jobDescription,
        RemoteWork remoteWork,
        List<ContractDetails> contractsDetails,
        OfficeLocation location,
        DateTimeOffset? publishedUntil,
        Requirements requirements) => new(id,
                                          companyDetails,
                                          offerSummary,
                                          jobDescription,
                                          remoteWork,
                                          contractsDetails,
                                          location,
                                          publishedUntil,
                                          requirements);

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
}