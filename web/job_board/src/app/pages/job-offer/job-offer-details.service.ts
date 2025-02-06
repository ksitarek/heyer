import { HttpClient } from '@angular/common/http';
import { computed, Inject, Injectable, ResourceRef, signal } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { catchError, map, Observable, of } from 'rxjs';
import { heyerApiUrl } from '../../app.config';
import { HttpErrorHandlerService } from '../../http-error-handler.service';
import { JobOfferDetails } from './job-offer-details.model';

@Injectable({
  providedIn: 'root',
})
export class JobOfferDetailsService {
  public readonly jobOfferId = signal('');
  public readonly jobOfferDetails = computed(() => this.detailsResource.value());
  public readonly detailsResource: ResourceRef<JobOfferDetails | null>;

  constructor(
    private http: HttpClient,
    private errorHandler: HttpErrorHandlerService,
    @Inject(heyerApiUrl) private api_url: string,
  ) {
    this.detailsResource = rxResource({
      request: () => ({
        id: this.jobOfferId(),
      }),

      loader: ({ request }) => {
        return this.fetch(request.id);
      },
    });
  }

  private fetch(id: string): Observable<JobOfferDetails | null> {
    return this.http.get<JobOfferDetails>(`${this.api_url}/job-board/${id}`).pipe(
      map((response) => JobOfferDetails.from(response)),
      catchError((error: unknown) => {
        this.errorHandler.handleError(error);
        return of(null);
      }),
    );
  }
}
