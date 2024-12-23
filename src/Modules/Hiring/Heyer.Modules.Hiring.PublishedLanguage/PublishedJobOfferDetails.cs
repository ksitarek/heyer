using System.Text.Json.Serialization;

namespace Heyer.Modules.Hiring.PublishedLanguage;

public record PublishedJobOfferDetails
{
    [JsonConstructor]
    public PublishedJobOfferDetails(Guid Id,
                                    CompanyDetails CompanyDetails,
                                    string OfferSummary,
                                    string JobDescription,
                                    OfficeLocation OfficeLocation,
                                    RemoteWork RemoteWork,
                                    Requirements Requirements,
                                    List<ContractDetails> ContractDetails)
    {
        this.Id = Id;
        this.CompanyDetails = CompanyDetails;
        this.OfferSummary = OfferSummary;
        this.JobDescription = JobDescription;
        this.OfficeLocation = OfficeLocation;
        this.RemoteWork = RemoteWork;
        this.Requirements = Requirements;
        this.ContractDetails = ContractDetails;
    }

    public Guid Id { get; init; }
    public CompanyDetails CompanyDetails { get; init; }
    public string OfferSummary { get; init; }
    public string JobDescription { get; init; }
    public OfficeLocation OfficeLocation { get; init; }
    public RemoteWork RemoteWork { get; init; }
    public Requirements Requirements { get; init; }
    public List<ContractDetails> ContractDetails { get; init; }

    public void Deconstruct(out Guid Id,
                            out CompanyDetails CompanyDetails,
                            out string OfferSummary,
                            out string JobDescription,
                            out OfficeLocation OfficeLocationD,
                            out RemoteWork RemoteWork,
                            out Requirements Requirements,
                            out List<ContractDetails> ContractDetails)
    {
        Id = this.Id;
        CompanyDetails = this.CompanyDetails;
        OfferSummary = this.OfferSummary;
        JobDescription = this.JobDescription;
        OfficeLocationD = OfficeLocation;
        RemoteWork = this.RemoteWork;
        Requirements = this.Requirements;
        ContractDetails = this.ContractDetails;
    }
}