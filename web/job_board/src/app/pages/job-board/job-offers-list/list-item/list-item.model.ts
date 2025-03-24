import { ContractDetails } from '../../../../models/contract-details.model';
import { RemoteWork } from '../../../../models/remote-work.model';
import { SalaryRange } from '../../../../models/salary-range.model';

export class ListItemModel {
  public constructor(
    public Id: string,
    public OfferSummary: string,
    public RemoteWork: RemoteWork,
    public ContractsDetails: ContractDetails[],
    public LocationCity: string,
    public LocationCountry: string,
    public CompanyName: string,
    public PublishedAt: Date,
  ) {}

  public static from(obj: ListItemModel) {
    return new ListItemModel(
      obj.Id,
      obj.OfferSummary,
      obj.RemoteWork,
      obj.ContractsDetails.map((contract: ContractDetails) => ListItemModel.mapToContractDetails(contract)),
      obj.LocationCity,
      obj.LocationCountry,
      obj.CompanyName,
      new Date(obj.PublishedAt),
    );
  }

  private static mapToContractDetails(obj: ContractDetails): ContractDetails {
    return new ContractDetails(
      obj.EmploymentType,
      new SalaryRange(obj.SalaryRange.IsPublished, obj.SalaryRange.From, obj.SalaryRange.To),
      obj.TimeNominator,
      obj.TimeDenominator,
    );
  }
}
