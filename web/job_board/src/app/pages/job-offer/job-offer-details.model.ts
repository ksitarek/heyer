import { ContractDetails } from '../../models/contract-details.model';
import { Requirements } from '../../models/requirements.model';
import { CompanyDetails, OfficeLocation } from './../../models/company-details.model';
import { RemoteWork } from './../../models/remote-work.model';

export class JobOfferDetails {
  public constructor(
    public Id: string,
    public CompanyDetails: CompanyDetails,
    public OfferSummary: string,
    public JobDescription: string,
    public OfficeLocation: OfficeLocation,
    public RemoteWork: RemoteWork,
    public Requirements: Requirements,
    public ContractsDetails: ContractDetails[],
  ) {}

  public static from(obj: JobOfferDetails) {
    return new JobOfferDetails(
      obj.Id,
      CompanyDetails.from(obj.CompanyDetails),
      obj.OfferSummary,
      obj.JobDescription,
      OfficeLocation.from(obj.OfficeLocation),
      obj.RemoteWork,
      Requirements.from(obj.Requirements),
      obj.ContractsDetails.map((contract: ContractDetails) => ContractDetails.from(contract)),
    );
  }
}
