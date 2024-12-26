using Heyer.BuildingBlocks.Infrastructure.Integration;
using Heyer.Modules.Hiring.PublishedLanguage.DTOs;

namespace Heyer.Modules.Hiring.PublishedLanguage.IntegrationEvents;

public record JobOfferPublishedIntegrationEvent : IntegrationEvent
{
    public JobOfferPublishedIntegrationEvent(Guid id,
                                             DateTime occurredOn,
                                             Guid jobOfferId,
                                             CompanyDetails companyDetails,
                                             string offerSummary,
                                             string jobDescription,
                                             RemoteWork remoteWork,
                                             List<ContractDetails> contractsDetails,
                                             OfficeLocation location,
                                             DateTimeOffset? publishedUntil,
                                             Requirements requirements) : base(id, occurredOn)
    {
        JobOfferId = jobOfferId;
        CompanyDetails = companyDetails;
        OfferSummary = offerSummary;
        JobDescription = jobDescription;
        RemoteWork = remoteWork;
        ContractsDetails = contractsDetails;
        Location = location;
        PublishedUntil = publishedUntil;
        Requirements = requirements;
    }

    public Guid JobOfferId { get; set; }
    public CompanyDetails CompanyDetails { get; set; }
    public string OfferSummary { get; set; }
    public string JobDescription { get; set; }
    public RemoteWork RemoteWork { get; set; }
    public List<ContractDetails> ContractsDetails { get; set; }
    public OfficeLocation Location { get; set; }
    public DateTimeOffset? PublishedUntil { get; set; }
    public Requirements Requirements { get; set; }
}