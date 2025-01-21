export class JobOfferDetails {
  constructor(
    public offerSummary: string,
    public jobDescription: string,
    public remoteWork: string,
    public publishedAt: Date,
    public publishedUntil: Date,
    public officeLocation: OfficeLocation,
    public requirements: Requirements,
    public contractsDetails: ContractDetails[]
  ) {}

  public static from(obj: any): JobOfferDetails {
    return new JobOfferDetails(
      obj.OfferSummary,

      obj.JobDescription,
      obj.RemoteWork,
      obj.PublishedAt,
      obj.PublishedUntil,
      OfficeLocation.from(obj.OfficeLocation),
      Requirements.from(obj.Requirements),
      obj.ContractsDetails.map((contractDetails: any) =>
        ContractDetails.from(contractDetails)
      )
    );
  }
}

export class OfficeLocation {
  constructor(public city: string, public country: string) {}

  public static from(obj: any | null): OfficeLocation {
    return new OfficeLocation(obj?.City, obj?.Country);
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
    public experienceLevel: ExperienceLevel,
    public skills: Skill[]
  ) {}

  public static from(obj: any): Requirements {
    return new Requirements(
      obj?.ExperienceLevel,
      obj?.Skills?.map((skill: any) => Skill.from(skill))
    );
  }
}

export class Skill {
  constructor(public label: string, public level: SkillLevel) {}

  public static from(obj: any): Skill {
    return new Skill(obj?.Label, obj?.Level);
  }
}

export class ContractDetails {
  constructor(
    public employmentType: EmploymentType,
    public salaryRange: SalaryRange,
    public timeNumerator: number,
    public timeDenumerator: number
  ) {}

  public static from(obj: any): ContractDetails {
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
    public isPublished: boolean,
    public from: number,
    public to: number
  ) {}

  public static from(obj: any): SalaryRange {
    return new SalaryRange(obj?.IsPublished, obj?.From, obj?.To);
  }
}
