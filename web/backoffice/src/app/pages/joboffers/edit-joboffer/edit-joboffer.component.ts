import { EmploymentType, JobOfferDetails } from './../joboffer-details';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PageHeaderComponent } from '../../../layout/components/page-header/page-header.component';
import {
  FormArray,
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { JobOfferForms } from '../form/joboffer-forms';
import { DescriptionFormComponent } from '../form/description-form/description-form.component';
import { LocationFormComponent } from '../form/location-form/location-form.component';
import { ContractsDetailsFormComponent } from '../form/contracts-details-form/contracts-details-form.component';
import { RequirementsFormComponent } from '../form/requirements-form/requirements-form.component';
import { JsonPipe } from '@angular/common';

@Component({
  selector: 'h-edit-joboffer',
  imports: [
    PageHeaderComponent,
    FormsModule,
    ReactiveFormsModule,
    DescriptionFormComponent,
    LocationFormComponent,
    ContractsDetailsFormComponent,
    RequirementsFormComponent,
  ],
  providers: [JobOfferForms],
  templateUrl: './edit-joboffer.component.html',
  styleUrl: './edit-joboffer.component.scss',
})
export class EditJobofferComponent implements OnInit {
  protected jobOffer!: JobOfferDetails;
  protected readonly jobOfferForm!: FormGroup;

  constructor(private route: ActivatedRoute, private forms: JobOfferForms) {
    this.jobOfferForm = this.forms.editJobOfferForm;
  }

  public ngOnInit(): void {
    this.contractsDetails.clear();
    this.skills.clear();

    this.route.data.subscribe((data) => {
      this.jobOffer = data['jobOffer'];

      this.jobOfferForm.patchValue({
        description: {
          offerSummary: this.jobOffer.offerSummary,
          jobDescription: this.jobOffer.jobDescription,
          remoteWork: this.jobOffer.remoteWork,
        },

        location: {
          city: this.jobOffer.officeLocation.city,
          country: this.jobOffer.officeLocation.country,
        },

        requirements: {
          experienceLevel: this.jobOffer.requirements?.experienceLevel,
        },
      });
    });

    for (const contractDetails of this.jobOffer.contractsDetails ?? []) {
      this.contractsDetails.push(
        this.forms.contractDetailsGroup(contractDetails)
      );
    }

    for (const skill of this.jobOffer.requirements.skills ?? []) {
      this.skills.push(this.forms.skillGroup(skill));
    }
  }
  public get contractsDetails(): FormArray {
    return this.jobOfferForm.get('contractsDetails') as FormArray;
  }

  public get skills(): FormArray {
    return this.jobOfferForm.get('requirements.skills') as FormArray;
  }
}
