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
