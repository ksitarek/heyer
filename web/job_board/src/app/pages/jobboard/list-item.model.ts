import { map } from 'rxjs';
export class ListItemModel {
  public constructor(
    public Id: string,
    public OfferSummary: string,
    public RemoteWork: RemoteWork,
    public ContractsDetails: ContractDetails[],
    public LocationCity: string,
    public LocationCountry: string,
    public CompanyName: string,
    public PublishedAt: Date
  ){}

  public static from(obj: any){return new ListItemModel(
    obj.Id,
    obj.OfferSummary,
    ListItemModel.mapToRemoteWork(obj.RemoteWork),
    obj.ContractsDetails.map((contract: any) => ListItemModel.mapToContractDetails(contract)),
    obj.LocationCity,
    obj.LocationCountry,
    obj.CompanyName,
    new Date(obj.PublishedAt)
  );
  }

  private static mapToRemoteWork(remoteWork: string): RemoteWork {
    switch (remoteWork) {
      case 'No':
        return RemoteWork.No;
      case 'Hybrid':
        return RemoteWork.Hybrid;
      case 'Yes':
        return RemoteWork.Yes;
      default:
        return RemoteWork.Unknown;
    }
  }
  private static mapToContractDetails(obj: any): ContractDetails {
    return new ContractDetails(
      ListItemModel.mapToEmploymentType(obj.EmploymentType),
      new SalaryRange(obj.SalaryRange.IsPublished, obj.SalaryRange.From, obj.SalaryRange.To),
      obj.TimeNominator,
      obj.TimeDenominator
    );
  }

  private static mapToEmploymentType(employmentType: string): EmploymentType {
    switch (employmentType) {
      case 'ContractOfEmployment':
        return EmploymentType.ContractOfEmployment;
      case 'B2B':
        return EmploymentType.B2B;
      default:
        return EmploymentType.ContractOfEmployment;
    }
  }
}

export enum RemoteWork {
  Unknown,
  No,
  Hybrid,
  Yes
}

export enum EmploymentType
{
    ContractOfEmployment,
    B2B
}

export class SalaryRange
{
    public constructor(
      public IsPublished: boolean,
      public From: number,
      public To: number
    ) {}
}

export class ContractDetails {
  public constructor(
    public EmploymentType: EmploymentType,
    public SalaryRange: SalaryRange,
    public TimeNominator: number,
    public TimeDenominator: number
  ){}
}
