import { Component, OnInit } from '@angular/core';
import {
  FormArray,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { PageHeaderComponent } from '../../../layout/components/page-header/page-header.component';
import { ContractsDetailsFormComponent } from '../form/contracts-details-form/contracts-details-form.component';
import { DescriptionFormComponent } from '../form/description-form/description-form.component';
import { JobOfferForms } from '../form/joboffer-forms';
import { LocationFormComponent } from '../form/location-form/location-form.component';
import { RequirementsFormComponent } from '../form/requirements-form/requirements-form.component';
import { JobOfferDetails } from './../joboffer-details';

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

  constructor(
    private route: ActivatedRoute,
    private forms: JobOfferForms,
  ) {
    this.jobOfferForm = this.forms.editJobOfferForm;
  }

  public ngOnInit(): void {
    this.contractsDetails.clear();
    this.skills.clear();

    this.route.data.subscribe((data) => {
      this.jobOffer = data['jobOffer'] as JobOfferDetails;

      this.jobOfferForm.patchValue({
        id: this.jobOffer.Id,

        description: {
          offerSummary: this.jobOffer.OfferSummary,
          jobDescription: this.jobOffer.JobDescription,
          remoteWork: this.jobOffer.RemoteWork,
        },

        location: {
          city: this.jobOffer.OfficeLocation.City,
          country: this.jobOffer.OfficeLocation.Country,
        },

        requirements: {
          experienceLevel: this.jobOffer.Requirements.ExperienceLevel,
        },
      });
    });

    for (const contractDetails of this.jobOffer.ContractsDetails) {
      this.contractsDetails.push(
        this.forms.contractDetailsGroup(contractDetails),
      );
    }

    for (const skill of this.jobOffer.Requirements.Skills) {
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
