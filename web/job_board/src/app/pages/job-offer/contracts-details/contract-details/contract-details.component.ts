import { Component, input } from '@angular/core';
import { ContractDetails } from '../../../../models/contract-details.model';
import { EmploymentType } from '../../../../models/employment-type.model';
import { SalaryRange } from '../../../../models/salary-range.model';
import { SalaryRangeComponent } from '../salary-range/salary-range.component';
import { HlmH3Directive } from './../../../../../../libs/ui/ui-typography-helm/src/lib/hlm-h3.directive';

@Component({
  selector: 'h-contract-details',
  imports: [HlmH3Directive, SalaryRangeComponent],
  templateUrl: './contract-details.component.html',
  styleUrl: './contract-details.component.scss',
})
export class ContractDetailsComponent {
  public readonly contractDetails = input.required<ContractDetails>();

  public get salaryRange(): SalaryRange {
    return this.contractDetails().SalaryRange;
  }

  public get employmentTypeLabel(): string {
    switch (this.contractDetails().EmploymentType) {
      case EmploymentType.B2B:
        return 'B2B';
      case EmploymentType.ContractOfEmployment:
        return 'Contract of employment';
    }
  }
}
