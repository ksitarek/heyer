import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, MaybeAsync, RedirectCommand, Resolve } from '@angular/router';
import { JobOfferDetails } from './joboffer-details';
import { JobOfferDetailsService } from './joboffers-details.service';

@Injectable({
  providedIn: 'root',
})
export class JobOfferResolver implements Resolve<JobOfferDetails> {
  constructor(private jobOfferDetailsService: JobOfferDetailsService) {}
  public resolve(route: ActivatedRouteSnapshot): MaybeAsync<JobOfferDetails | RedirectCommand> {
    const id = this.getIdFromRoute(route);

    return this.jobOfferDetailsService.getJobOfferDetails(id);
  }

  private getIdFromRoute(route: ActivatedRouteSnapshot): string {
    const params = route.params as Record<string, unknown>;

    const id = params['id'];

    return id?.toString() ?? '';
  }
}
