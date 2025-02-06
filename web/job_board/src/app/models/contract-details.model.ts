import { EmploymentType } from './employment-type.model';
import { SalaryRange } from './salary-range.model';

export class ContractDetails {
  public constructor(
    public EmploymentType: EmploymentType,
    public SalaryRange: SalaryRange,
    public TimeNominator: number,
    public TimeDenominator: number,
  ) {}

  public static from(obj: ContractDetails) {
    return new ContractDetails(
      obj.EmploymentType,
      SalaryRange.from(obj.SalaryRange),
      obj.TimeNominator,
      obj.TimeDenominator,
    );
  }
}
