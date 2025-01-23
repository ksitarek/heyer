import { inject, Injectable } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { ContractDetails, Skill, SkillLevel } from '../joboffer-details';
import { RemoteWork } from '../remote-work-control/remote-work';

@Injectable({
  providedIn: 'root',
})
export class JobOfferForms {
  private readonly fb: FormBuilder = inject(FormBuilder);

  public readonly descriptionGroup = this.fb.group({
    offerSummary: new FormControl('', [
      Validators.required,
      Validators.minLength(10),
      Validators.maxLength(100),
    ]),
    jobDescription: new FormControl('', [
      Validators.required,
      Validators.minLength(100),
    ]),
    remoteWork: new FormControl(RemoteWork.Hybrid),
  });

  public contractDetailsGroup(values: ContractDetails | null = null) {
    return this.fb.group({
      employmentType: new FormControl(values?.EmploymentType ?? '', []),
      salaryRange: this.fb.group({
        from: new FormControl(values?.SalaryRange.From ?? '', [
          Validators.min(0),
        ]),
        to: new FormControl(values?.SalaryRange.To ?? '', [Validators.min(0)]), // todo add "greater than" validation
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

    // location: this.fb.group({
    //   city: new FormControl('', []),
    //   country: new FormControl('', []),
    // }),

    // contractsDetails: this.fb.array([this.contractDetailsGroup()]),

    // requirements: this.fb.group({
    //   experienceLevel: new FormControl('', []),
    //   skills: this.fb.array([this.skillGroup()]),
    // }),
  });

  public readonly editJobOfferForm = this.fb.group({
    description: this.descriptionGroup,

    location: this.fb.group({
      city: new FormControl('', []),
      country: new FormControl('', []),
    }),

    contractsDetails: this.fb.array([this.contractDetailsGroup()]),

    requirements: this.fb.group({
      experienceLevel: new FormControl('', []),
      skills: this.fb.array([this.skillGroup()]),
    }),
  });
}
