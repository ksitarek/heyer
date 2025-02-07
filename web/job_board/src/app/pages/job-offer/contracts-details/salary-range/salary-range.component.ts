import { Component, input } from '@angular/core';
import { SalaryRange } from '../../../../models/salary-range.model';

@Component({
  selector: 'h-salary-range',
  imports: [],
  templateUrl: './salary-range.component.html',
  styleUrl: './salary-range.component.scss',
})
export class SalaryRangeComponent {
  public readonly salaryRange = input.required<SalaryRange>();

  public get range() {
    return this.salaryRange();
  }
}
