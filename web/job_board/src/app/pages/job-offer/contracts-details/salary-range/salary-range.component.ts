import { Component, input } from '@angular/core';
import { HlmH2Directive } from '@spartan-ng/ui-typography-helm';
import { SalaryRange } from '../../../../models/salary-range.model';

@Component({
  selector: 'h-salary-range',
  imports: [HlmH2Directive],
  templateUrl: './salary-range.component.html',
  styleUrl: './salary-range.component.scss',
})
export class SalaryRangeComponent {
  public readonly salaryRange = input.required<SalaryRange>();

  public get range() {
    return this.salaryRange();
  }
}
