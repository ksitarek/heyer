import { inject, Injectable } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, ValidationErrors, Validators } from '@angular/forms';
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
      salaryRange: this.fb.group(
        {
          from: new FormControl(values?.SalaryRange.From ?? 0, [Validators.min(1)]),
          to: new FormControl(values?.SalaryRange.To ?? 0, [Validators.min(1)]),
          isPublished: new FormControl(values?.SalaryRange.IsPublished ?? true),
        },
        { validators: this.salaryRangeValidator },
      ),
    });
  }

  public skillGroup(values: Skill | null = null) {
    return this.fb.group({
      label: new FormControl(values?.Label ?? '', [Validators.required]),
      level: new FormControl(values?.Level ?? SkillLevel.NiceToHave, [Validators.required]),
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
      experienceLevel: new FormControl('', [Validators.required]),
      skills: this.fb.array([this.skillGroup()]),
    }),
  });

  salaryRangeValidator = (group: AbstractControl): ValidationErrors | null => {
    const fromControl = group.get('from');
    const toControl = group.get('to');
    const from = fromControl?.value as number;
    const to = toControl?.value as number;

    if (from >= to) {
      fromControl?.setErrors({ invalidRange: true });
      toControl?.setErrors({ invalidRange: true });

      return { invalidRange: true };
    }

    fromControl?.setErrors(null);
    toControl?.setErrors(null);

    return null;
  };
}
