import { Component, input, OnDestroy, OnInit } from '@angular/core';
import { AbstractControl, FormArray, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { NgIcon } from '@ng-icons/core';
import { BrnMenuTriggerDirective } from '@spartan-ng/brain/menu';
import { HlmButtonDirective } from '@spartan-ng/ui-button-helm';
import { HlmInputDirective } from '@spartan-ng/ui-input-helm';
import { HlmLabelDirective } from '@spartan-ng/ui-label-helm';
import {
  HlmMenuComponent,
  HlmMenuGroupComponent,
  HlmMenuItemDirective,
  HlmMenuItemIconDirective,
} from '@spartan-ng/ui-menu-helm';
import { debounceTime, distinct, filter, Subscription, switchMap, take, tap } from 'rxjs';
import { HlmCheckboxComponent } from '../../../../../../libs/ui/ui-checkbox-helm/src/lib/hlm-checkbox.component';
import { JobOfferDetailsService } from '../../joboffers-details.service';
import { JobOfferForms } from '../joboffer-forms';
import { ContractDetails, EmploymentType, SalaryRange } from './../../joboffer-details';

@Component({
  selector: 'h-contracts-details-form',
  imports: [
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
export class ContractsDetailsFormComponent implements OnInit, OnDestroy {
  readonly form = input.required<FormGroup>();

  private tempEmploymentTypes: EmploymentType[] = [];

  constructor(
    private jobOfferForms: JobOfferForms,
    private jobOfferDetailsService: JobOfferDetailsService,
  ) {}

  public ngOnInit(): void {
    this.contractsDetails.controls.forEach((control) => {
      this.subscribeToContractDetailsChanges(control as FormGroup);
    });
  }

  public ngOnDestroy(): void {
    this.contractDetailsSubscriptions.forEach((subscription) => {
      subscription.unsubscribe();
    });
  }

  public get contractsDetails(): FormArray {
    return this.form().get('contractsDetails') as FormArray;
  }

  public getEmploymentTypeLabelByControlValue(employmentTypeControl: AbstractControl): string {
    const employmentType = employmentTypeControl.value as EmploymentType;

    return this.getEmploymentTypeLabel(employmentType);
  }

  public getEmploymentTypeLabel(employmentType: EmploymentType): string {
    switch (employmentType) {
      case EmploymentType.B2B:
        return 'B2B';
      case EmploymentType.ContractOfEmployment:
        return 'Contract of employment';
    }
  }

  public removeContract(i: number): void {
    console.log(this.contractsDetails.controls);

    const detailsToRemove = this.contractsDetails.controls[i];
    const employmentType = detailsToRemove.get('employmentType')?.value as EmploymentType;

    if (this.hasTempEmploymentType(employmentType)) {
      // This contract was not yet saved to the server
      this.contractsDetails.controls.splice(i, 1);
      this.removeTempEmploymentType(employmentType);
    } else {
      this.jobOfferDetailsService
        .removeContractDetails(this.jobOfferId, employmentType)
        .pipe(
          take(1),
          tap(() => {
            this.contractsDetails.controls.splice(i, 1);
          }),
        )
        .subscribe();
    }
  }

  public get availableContractTypes(): EmploymentType[] {
    const usedEmploymentTypes = this.contractsDetails.controls.map(
      (control) => control.get('employmentType')?.value as EmploymentType,
    );

    return Object.values(EmploymentType).filter((value) => !usedEmploymentTypes.includes(value));
  }

  public addContract(type: EmploymentType): void {
    const contractDetailsGroup = this.jobOfferForms.contractDetailsGroup(null);

    contractDetailsGroup.get('employmentType')?.setValue(type);

    this.contractsDetails.push(contractDetailsGroup);

    this.storeTempEmploymentType(type);

    contractDetailsGroup.valueChanges
      .pipe(
        debounceTime(500),
        distinct(),
        filter(() => contractDetailsGroup.dirty),
        filter(() => contractDetailsGroup.valid),

        switchMap((contractDetails) =>
          this.jobOfferDetailsService.addContractDetails(
            this.jobOfferId,
            new ContractDetails(
              contractDetails.employmentType as EmploymentType,
              new SalaryRange(
                contractDetails.salaryRange?.isPublished ?? false,
                contractDetails.salaryRange?.from ?? 0,
                contractDetails.salaryRange?.to ?? 0,
              ),
              8,
              8,
            ),
          ),
        ),
        take(1),
        tap(() => {
          this.removeTempEmploymentType(type);
          this.subscribeToContractDetailsChanges(contractDetailsGroup);
        }),
      )
      .subscribe();
  }

  private get jobOfferId(): string {
    return this.form().get('id')?.value as string;
  }

  private storeTempEmploymentType(type: EmploymentType): void {
    this.tempEmploymentTypes.push(type);
  }

  private removeTempEmploymentType(type: EmploymentType): void {
    this.tempEmploymentTypes = this.tempEmploymentTypes.filter((value) => value !== type);
  }

  private hasTempEmploymentType(type: EmploymentType): boolean {
    return this.tempEmploymentTypes.includes(type);
  }

  private contractDetailsSubscriptions: Subscription[] = [];

  private subscribeToContractDetailsChanges(formGroup: FormGroup): void {
    const subscription = formGroup.valueChanges
      .pipe(
        debounceTime(500),
        distinct(),
        filter(() => formGroup.dirty),
        filter(() => formGroup.valid),
        switchMap(
          (contractDetails: {
            employmentType: EmploymentType;
            salaryRange: { isPublished: boolean; from: number; to: number };
          }) =>
            this.jobOfferDetailsService.updateContractDetails(
              this.jobOfferId,
              new ContractDetails(
                contractDetails.employmentType,
                new SalaryRange(
                  contractDetails.salaryRange.isPublished,
                  contractDetails.salaryRange.from,
                  contractDetails.salaryRange.to,
                ),
                8,
                8,
              ),
            ),
        ),
      )
      .subscribe();

    this.contractDetailsSubscriptions.push(subscription);
  }
}
