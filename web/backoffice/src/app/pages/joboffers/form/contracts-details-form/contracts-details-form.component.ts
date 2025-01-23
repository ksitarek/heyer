import { EmploymentType } from './../../joboffer-details';
import { Component, Input } from '@angular/core';
import {
  AbstractControl,
  FormArray,
  FormGroup,
  ReactiveFormsModule,
} from '@angular/forms';
import { JobOfferForms } from '../joboffer-forms';
import { NgFor, NgIf } from '@angular/common';
import { HlmLabelDirective } from '@spartan-ng/ui-label-helm';
import { HlmInputDirective } from '@spartan-ng/ui-input-helm';
import { HlmCheckboxComponent } from '../../../../../../libs/ui/ui-checkbox-helm/src/lib/hlm-checkbox.component';
import { NgIcon } from '@ng-icons/core';
import { HlmButtonDirective } from '@spartan-ng/ui-button-helm';
import { BrnMenuTriggerDirective } from '@spartan-ng/brain/menu';
import {
  HlmMenuComponent,
  HlmMenuGroupComponent,
  HlmMenuItemDirective,
  HlmMenuItemIconDirective,
} from '@spartan-ng/ui-menu-helm';

@Component({
  selector: 'h-contracts-details-form',
  imports: [
    NgFor,
    NgIf,
    NgIcon,
    ReactiveFormsModule,
    HlmLabelDirective,
    HlmInputDirective,
    HlmButtonDirective,
    HlmCheckboxComponent,

    BrnMenuTriggerDirective,

    HlmMenuGroupComponent,
    HlmMenuComponent,
    HlmMenuItemDirective,
    HlmMenuItemIconDirective,
  ],
  providers: [JobOfferForms],
  templateUrl: './contracts-details-form.component.html',
  styleUrl: './contracts-details-form.component.scss',
})
export class ContractsDetailsFormComponent {
  @Input({ required: true }) form!: FormGroup;

  constructor(private jobOfferForms: JobOfferForms) {}

  public get contractsDetails(): FormArray {
    return this.form.get('contractsDetails') as FormArray;
  }

  public getEmploymentTypeLabelByControlValue(
    employmentTypeControl: AbstractControl,
  ): string {
    const employmentType = employmentTypeControl.value;
    return this.getEmploymentTypeLabel(employmentType);
  }

  public getEmploymentTypeLabel(employmentType: EmploymentType): string {
    switch (employmentType) {
      case EmploymentType.B2B:
        return 'B2B';
      case EmploymentType.ContractOfEmployment:
        return 'Contract of employment';
    }

    return 'UNKNOWN';
  }

  public removeContract(i: number): void {
    this.contractsDetails.controls.splice(i, 1);
  }

  public get availableContractTypes(): EmploymentType[] {
    const usedEmploymentTypes = this.contractsDetails.controls.map(
      (control) => control.get('employmentType')?.value,
    );

    return Object.values(EmploymentType).filter(
      (value) => !usedEmploymentTypes.includes(value),
    );
  }

  public addContract(type: EmploymentType): void {
    const contractDetailsGroup = this.jobOfferForms.contractDetailsGroup(null);

    contractDetailsGroup.get('employmentType')?.setValue(type);

    this.contractsDetails.push(contractDetailsGroup);
  }
}
