import { inject, Injectable } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { ContractDetails, Skill, SkillLevel } from '../joboffer-details';
import { RemoteWork } from '../remote-work-control/remote-work';

@Injectable({
  providedIn: 'root',
})
export class JobOfferForms {
  private readonly fb: FormBuilder = inject(FormBuilder);

  public readonly descriptionGroup = this.fb.group({
    offerSummary: new FormControl('', [Validators.required, Validators.minLength(10), Validators.maxLength(100)]),
    jobDescription: new FormControl('', [Validators.required, Validators.minLength(100)]),
    remoteWork: new FormControl(RemoteWork.Hybrid),
  });

  public contractDetailsGroup(values: ContractDetails | null = null) {
    return this.fb.group({
      employmentType: new FormControl(values?.EmploymentType ?? '', []),
      salaryRange: this.fb.group({
        from: new FormControl(values?.SalaryRange.From ?? '', [Validators.min(0)]),
        to: new FormControl(values?.SalaryRange.To ?? '', [Validators.min(0), this.greaterThan('from')]),
        isPublished: new FormControl(values?.SalaryRange.IsPublished ?? true),
      }),
    });
  }

  public skillGroup(values: Skill | null = null) {
    return this.fb.group({
      label: new FormControl(values?.Label ?? '', []),
      level: new FormControl(values?.Level ?? SkillLevel.NiceToHave, []),
    });
  }

  public readonly createJobOfferForm = this.fb.group({
    description: this.descriptionGroup,
  });

  public readonly editJobOfferForm = this.fb.group({
    id: new FormControl('', []),
    description: this.descriptionGroup,

    location: this.fb.group({
      city: new FormControl('', [Validators.required]),
      country: new FormControl('', [Validators.required]),
    }),

    contractsDetails: this.fb.array([this.contractDetailsGroup()]),

    requirements: this.fb.group({
      experienceLevel: new FormControl('', []),
      skills: this.fb.array([this.skillGroup()]),
    }),
  });

  greaterThan(fieldKey: string): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const group = control.parent;
      if (!group) {
        return null;
      }

      const field = group.get(fieldKey);
      if (!field) {
        return null;
      }

      const value = control.value as number;
      const referenceValue = field.value as number;

      if (value < referenceValue) {
        return { greaterThan: true };
      }

      return null;
    };
  }
}
