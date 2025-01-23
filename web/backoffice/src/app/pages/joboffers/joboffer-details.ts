export class JobOfferDetails {
  constructor(
    public OfferSummary: string,
    public JobDescription: string,
    public RemoteWork: string,
    public PublishedAt: Date,
    public PublishedUntil: Date,
    public OfficeLocation: OfficeLocation,
    public Requirements: Requirements,
    public ContractsDetails: ContractDetails[]
  ) {}

  public static from(obj: JobOfferDetails): JobOfferDetails {
    return new JobOfferDetails(
      obj.OfferSummary,

      obj.JobDescription,
      obj.RemoteWork,
      obj.PublishedAt,
      obj.PublishedUntil,
      OfficeLocation.from(obj.OfficeLocation),
      Requirements.from(obj.Requirements),
      obj.ContractsDetails.map((contractDetails: ContractDetails) =>
        ContractDetails.from(contractDetails)
      )
    );
  }
}

export class OfficeLocation {
  constructor(public City: string, public Country: string) {}

  public static from(obj: OfficeLocation | null): OfficeLocation {
    return new OfficeLocation(obj?.City ?? '', obj?.Country ?? '');
  }
}

export enum ExperienceLevel {
  Junior = 'Junior',
  Mid = 'Mid',
  Senior = 'Senior',
  CLevel = 'CLevel',
}

export enum SkillLevel {
  NiceToHave = 'NiceToHave',
  Junior = 'Junior',
  Mid = 'Mid',
  Senior = 'Senior',
  Expert = 'Expert',
}

export enum EmploymentType {
  ContractOfEmployment = 'ContractOfEmployment',
  B2B = 'B2B',
}

export class Requirements {
  constructor(
    public ExperienceLevel: ExperienceLevel,
    public Skills: Skill[]
  ) {}

  public static from(obj: Requirements): Requirements {
    return new Requirements(
      obj?.ExperienceLevel,
      obj?.Skills?.map((skill: Skill) => Skill.from(skill))
    );
  }
}

export class Skill {
  constructor(public Label: string, public Level: SkillLevel) {}

  public static from(obj: Skill): Skill {
    return new Skill(obj?.Label, obj?.Level);
  }
}

export class ContractDetails {
  constructor(
    public EmploymentType: EmploymentType,
    public SalaryRange: SalaryRange,
    public TimeNumerator: number,
    public TimeDenumerator: number
  ) {}

  public static from(obj: ContractDetails): ContractDetails {
    return new ContractDetails(
      obj?.EmploymentType,
      SalaryRange.from(obj?.SalaryRange),
      obj?.TimeNumerator,
      obj?.TimeDenumerator
    );
  }
}

export class SalaryRange {
  constructor(
    public IsPublished: boolean,
    public From: number,
    public To: number
  ) {}

  public static from(obj: SalaryRange): SalaryRange {
    return new SalaryRange(obj?.IsPublished, obj?.From, obj?.To);
  }
}
