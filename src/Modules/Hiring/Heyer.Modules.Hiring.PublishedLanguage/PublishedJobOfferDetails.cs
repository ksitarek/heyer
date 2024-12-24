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

    public void Deconstruct(out Guid id,
                            out CompanyDetails companyDetails,
                            out string offerSummary,
                            out string jobDescription,
                            out OfficeLocation officeLocationD,
                            out RemoteWork remoteWork,
                            out Requirements requirements,
                            out List<ContractDetails> contractDetails)
    {
        id = this.Id;
        companyDetails = this.CompanyDetails;
        offerSummary = this.OfferSummary;
        jobDescription = this.JobDescription;
        officeLocationD = OfficeLocation;
        remoteWork = this.RemoteWork;
        requirements = this.Requirements;
        contractDetails = this.ContractDetails;
    }
}