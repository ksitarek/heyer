import { Component, effect, inject, signal } from '@angular/core';
import { PageHeaderComponent } from '../../../layout/components/page-header/page-header.component';
import { NgIcon } from '@ng-icons/core';
import { HlmButtonDirective } from '@spartan-ng/ui-button-helm';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { CreateJobOfferService } from './create-job-offer.service';
import { Router } from '@angular/router';
import { JobOfferForms } from '../form/joboffer-forms';
import { DescriptionFormComponent } from '../form/description-form/description-form.component';

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

    const description = this.jobOfferForm.get('description')!.value;

    this.createJobOfferService
      .saveDraft(
        description.offerSummary,
        description.jobDescription,
        description.remoteWork,
      )
      .subscribe((res) => {
        if (res.length > 0) {
          this.router.navigate(['/job-offers', 'edit', res]);
        } else {
          this.saveInProgress.set(false);
        }
      });
  }

  public get submitButtonDisabled(): boolean {
    this.jobOfferForm.updateValueAndValidity();

    return !this.jobOfferForm.valid || this.saveInProgress();
  }
}
