import {
  ActivatedRouteSnapshot,
  MaybeAsync,
  RedirectCommand,
  Resolve,
} from '@angular/router';
import { JobOfferDetails } from './joboffer-details';
import { JobOfferDetailsService } from './joboffers-details.service';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class JobOfferResolver implements Resolve<JobOfferDetails> {
  constructor(private jobOfferDetailsService: JobOfferDetailsService) {}
  public resolve(
    route: ActivatedRouteSnapshot,
  ): MaybeAsync<JobOfferDetails | RedirectCommand> {
    const id = route.params['id'];

    return this.jobOfferDetailsService.getJobOfferDetails(id);
  }
}
