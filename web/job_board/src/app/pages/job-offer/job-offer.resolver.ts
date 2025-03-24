import { Injectable } from '@angular/core';
import { toObservable } from '@angular/core/rxjs-interop';
import { ActivatedRouteSnapshot, MaybeAsync, RedirectCommand, Resolve, Router } from '@angular/router';
import { filter, map, Observable, take } from 'rxjs';
import { JobOfferDetails } from './job-offer-details.model';
import { JobOfferDetailsService } from './job-offer-details.service';

@Injectable({
  providedIn: 'root',
})
export class JobOfferResolver implements Resolve<JobOfferDetails> {
  private readonly details$: Observable<JobOfferDetails | null>;

  constructor(
    private jobOfferDetailsService: JobOfferDetailsService,
    private router: Router,
  ) {
    this.details$ = toObservable(this.jobOfferDetailsService.detailsResource.value).pipe(
      filter((details) => details !== undefined),
    );
  }

  public resolve(route: ActivatedRouteSnapshot): MaybeAsync<JobOfferDetails | RedirectCommand> {
    const id = this.getIdFromRoute(route);

    this.jobOfferDetailsService.jobOfferId.set(id);

    return this.details$.pipe(
      filter((details) => details?.Id === id),
      take(1),
      map((details) => details ?? this.redirectTo404()),
    );
  }

  private getIdFromRoute(route: ActivatedRouteSnapshot): string {
    const params = route.params as Record<string, unknown>;

    const id = params['id'];

    return id?.toString() ?? '';
  }

  private redirectTo404(): RedirectCommand {
    return new RedirectCommand(this.router.parseUrl('/not-found'));
  }
}
