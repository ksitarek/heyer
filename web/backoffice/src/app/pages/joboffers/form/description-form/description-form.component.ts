import { Component, input, OnDestroy, OnInit } from '@angular/core';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { HlmInputDirective } from '@spartan-ng/ui-input-helm';
import { HlmLabelDirective } from '@spartan-ng/ui-label-helm';
import { debounceTime, distinct, filter, Subscription, switchMap } from 'rxjs';
import { JobOfferDetailsService } from '../../joboffers-details.service';
import { RemoteWork } from '../../remote-work-control/remote-work';
import { JobOfferForms } from '../joboffer-forms';
import { RemoteWorkControlComponent } from './../../remote-work-control/remote-work-control.component';

@Component({
  selector: 'h-description-form',
  imports: [RemoteWorkControlComponent, ReactiveFormsModule, HlmLabelDirective, HlmInputDirective],
  providers: [JobOfferForms],
  templateUrl: './description-form.component.html',
  styleUrl: './description-form.component.scss',
})
export class DescriptionFormComponent implements OnInit, OnDestroy {
  readonly form = input.required<FormGroup>();
  readonly autosave = input<boolean>(false);

  private descriptionSubscription?: Subscription;

  constructor(private jobOfferDetailsService: JobOfferDetailsService) {}

  public ngOnInit(): void {
    const description$ = this.description.valueChanges;

    this.descriptionSubscription = description$
      .pipe(
        debounceTime(500),
        distinct(),

        filter(() => this.autosave()),
        filter(() => this.description.dirty),
        filter(() => this.description.valid),

        switchMap(() =>
          this.jobOfferDetailsService.updateJobOfferDescription(
            this.form().get('id')?.value as string,
            this.description.get('offerSummary')?.value as string,
            this.description.get('jobDescription')?.value as string,
            this.description.get('remoteWork')?.value as RemoteWork,
          ),
        ),
      )
      .subscribe();
  }

  public ngOnDestroy(): void {
    this.descriptionSubscription?.unsubscribe();
  }

  private get description(): FormGroup {
    return this.form().get('description') as FormGroup;
  }
}
