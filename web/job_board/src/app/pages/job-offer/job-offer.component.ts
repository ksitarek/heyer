import { Component, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HlmCardContentDirective, HlmCardDirective } from '@spartan-ng/ui-card-helm';
import { HlmH3Directive } from '@spartan-ng/ui-typography-helm';
import { ApplyButtonComponent } from './apply-button/apply-button.component';
import { ContractsDetailsComponent } from './contracts-details/contracts-details.component';
import { JobOfferDetails } from './job-offer-details.model';
import { RemoteWorkIndicatorComponent } from './remote-work-indicator/remote-work-indicator.component';
import { RequirementsComponent } from './requirements/requirements.component';

@Component({
  selector: 'h-job-offer',
  imports: [
    HlmH3Directive,
    HlmCardDirective,
    HlmCardContentDirective,
    ContractsDetailsComponent,
    RemoteWorkIndicatorComponent,
    RequirementsComponent,
    ApplyButtonComponent,
  ],
  templateUrl: './job-offer.component.html',
  styleUrl: './job-offer.component.scss',
})
export class JobOfferComponent {
  protected readonly detailsSignal = signal<JobOfferDetails>({} as JobOfferDetails);

  constructor(private route: ActivatedRoute) {
    this.detailsSignal.set(this.route.snapshot.data['jobOffer'] as JobOfferDetails);
  }

  public get details() {
    return this.detailsSignal();
  }
}
