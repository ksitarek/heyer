import { Component, effect, inject, signal } from '@angular/core';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { NgIcon } from '@ng-icons/core';
import { HlmButtonDirective } from '@spartan-ng/ui-button-helm';
import { tap } from 'rxjs';
import { PageHeaderComponent } from '../../../layout/components/page-header/page-header.component';
import { DescriptionFormComponent } from '../form/description-form/description-form.component';
import { JobOfferForms } from '../form/joboffer-forms';
import { RemoteWork } from '../remote-work-control/remote-work';
import { CreateJobOfferService } from './create-job-offer.service';

@Component({
  selector: 'h-create-joboffer',
  imports: [
    NgIcon,
    HlmButtonDirective,
    PageHeaderComponent,
    ReactiveFormsModule,
    DescriptionFormComponent,
  ],
  providers: [JobOfferForms],
  templateUrl: './create-joboffer.component.html',
  styleUrl: './create-joboffer.component.scss',
})
export class CreateJobofferComponent {
  protected readonly jobOfferForm: FormGroup =
    inject(JobOfferForms).createJobOfferForm;

  protected readonly saveInProgress = signal(false);
  protected readonly saveInProgressEffect = effect(() => {
    if (this.saveInProgress()) {
      this.jobOfferForm.disable();
    } else {
      this.jobOfferForm.enable();
    }
  });

  constructor(
    private createJobOfferService: CreateJobOfferService,
    private router: Router,
  ) {}

  public saveDraft(): void {
    this.saveInProgress.set(true);

    const descriptionControl = this.jobOfferForm.get('description');
    if (!descriptionControl) {
      console.error('Description control not found');
      return;
    }

    const description = descriptionControl.value as {
      offerSummary: string;
      jobDescription: string;
      remoteWork: RemoteWork;
    };

    if (!description.offerSummary || !description.jobDescription) {
      console.error('Description is missing required fields');
      return;
    }

    this.createJobOfferService
      .saveDraft(
        description.offerSummary,
        description.jobDescription,
        description.remoteWork,
      )
      .pipe(
        tap((res) => {
          if (res.length > 0) {
            void this.router.navigate(['/job-offers', 'edit', res]);
          } else {
            this.saveInProgress.set(false);
          }
        }),
      )
      .subscribe();
  }

  public get submitButtonDisabled(): boolean {
    this.jobOfferForm.updateValueAndValidity();

    return !this.jobOfferForm.valid || this.saveInProgress();
  }
}
