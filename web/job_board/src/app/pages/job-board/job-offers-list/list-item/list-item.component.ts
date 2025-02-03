import { Component, input } from '@angular/core';
import { NgIcon } from '@ng-icons/core';
import {
  HlmCardDescriptionDirective,
  HlmCardDirective,
  HlmCardHeaderDirective,
  HlmCardTitleDirective,
} from '@spartan-ng/ui-card-helm';
import { EmploymentType, ListItemModel, RemoteWork } from './list-item.model';

@Component({
  selector: 'h-list-item',
  imports: [NgIcon, HlmCardDirective, HlmCardHeaderDirective, HlmCardTitleDirective, HlmCardDescriptionDirective],
  templateUrl: './list-item.component.html',
  styleUrl: './list-item.component.scss',
})
export class ListItemComponent {
  public readonly item = input.required<ListItemModel>();

  public get minSalary(): number {
    const min = this.item().ContractsDetails.reduce((c1, c2) => {
      return c1.SalaryRange.IsPublished && c1.SalaryRange.From < c2.SalaryRange.From ? c1 : c2;
    });

    return min.SalaryRange.From;
  }

  public get maxSalary(): number {
    const max = this.item().ContractsDetails.reduce((c1, c2) => {
      return c1.SalaryRange.IsPublished && c1.SalaryRange.To > c2.SalaryRange.To ? c1 : c2;
    });

    return max.SalaryRange.To;
  }

  public get isB2B(): boolean {
    return this.item().ContractsDetails.some((c) => c.EmploymentType === EmploymentType.B2B);
  }

  public get isContractOfEmployment(): boolean {
    return this.item().ContractsDetails.some((c) => c.EmploymentType === EmploymentType.ContractOfEmployment);
  }

  public get isRemote(): boolean {
    return this.item().RemoteWork !== RemoteWork.No;
  }

  public get isHybrid(): boolean {
    return this.item().RemoteWork === RemoteWork.Hybrid;
  }
}
