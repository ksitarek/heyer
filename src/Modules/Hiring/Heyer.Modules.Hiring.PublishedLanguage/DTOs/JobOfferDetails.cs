using System.Text.Json.Serialization;

namespace Heyer.Modules.Hiring.PublishedLanguage.DTOs;

public record JobOfferDetails
{
    [JsonConstructor]
    public JobOfferDetails(Guid Id,
                           string OfferSummary,
                           string JobDescription,
                           DateTimeOffset? PublishedAt,
                           DateTimeOffset? PublishedUntil,
                           OfficeLocation OfficeLocation,
                           RemoteWork RemoteWork,
                           Requirements Requirements,
                           List<ContractDetails> ContractsDetails)
    {
        this.Id = Id;
        this.OfferSummary = OfferSummary;
        this.JobDescription = JobDescription;
        this.PublishedAt = PublishedAt;
        this.PublishedUntil = PublishedUntil;
        this.OfficeLocation = OfficeLocation;
        this.RemoteWork = RemoteWork;
        this.Requirements = Requirements;
        this.ContractsDetails = ContractsDetails;
    }

    public Guid Id { get; init; }
    public string OfferSummary { get; init; }
    public string JobDescription { get; init; }
    public DateTimeOffset? PublishedAt { get; init; }
    public DateTimeOffset? PublishedUntil { get; init; }
    public OfficeLocation OfficeLocation { get; init; }
    public RemoteWork RemoteWork { get; init; }
    public Requirements Requirements { get; init; }
    public List<ContractDetails> ContractsDetails { get; init; }

    public void Deconstruct(out Guid Id,
                            out string OfferSummary,
                            out string JobDescription,
                            out DateTimeOffset? PublishedAt,
                            out DateTimeOffset? PublishedUntil,
                            out OfficeLocation OfficeLocation,
                            out RemoteWork RemoteWork,
                            out Requirements Requirements,
                            out List<ContractDetails> ContractDetails)
    {
        Id = this.Id;
        OfferSummary = this.OfferSummary;
        JobDescription = this.JobDescription;
        PublishedAt = this.PublishedAt;
        PublishedUntil = this.PublishedUntil;
        OfficeLocation = this.OfficeLocation;
        RemoteWork = this.RemoteWork;
        Requirements = this.Requirements;
        ContractDetails = ContractsDetails;
    }
}