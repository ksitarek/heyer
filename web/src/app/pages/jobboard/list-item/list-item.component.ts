import { RemoteWork, EmploymentType } from './../list-item.model';
import { Component, Input } from '@angular/core';
import { ListItemModel } from '../list-item.model';
import { JsonPipe, NgIf } from '@angular/common';
import { ListItemTagComponent } from "../list-item-tag/list-item-tag.component";

@Component({
  selector: 'app-list-item',
  imports: [NgIf, ListItemTagComponent],
  templateUrl: './list-item.component.html',
  styleUrl: './list-item.component.scss'
})
export class ListItemComponent {
  @Input({required: true}) item!: ListItemModel;

  public get minSalary(): number {
    var min = this.item.ContractsDetails.reduce((c1, c2) => {
      return c1.SalaryRange.IsPublished && c1.SalaryRange.From < c2.SalaryRange.From ? c1 : c2;
    });

    return min.SalaryRange.From;
  }

  public get maxSalary(): number {
    var max = this.item.ContractsDetails.reduce((c1, c2) => {
      return c1.SalaryRange.IsPublished && c1.SalaryRange.From > c2.SalaryRange.From ? c1 : c2;
    });

    return max.SalaryRange.From;
  }

  public get isB2B(): boolean {
    return this.item.ContractsDetails.some(c => c.EmploymentType === EmploymentType.B2B);
  }

  public get isContractOfEmployment(): boolean {
    return this.item.ContractsDetails.some(c => c.EmploymentType === EmploymentType.ContractOfEmployment);
  }

  public get isRemote(): boolean {
    return this.item.RemoteWork !== RemoteWork.No;
  }

  public get isHybrid(): boolean {
    return this.item.RemoteWork === RemoteWork.Hybrid;
  }
}
