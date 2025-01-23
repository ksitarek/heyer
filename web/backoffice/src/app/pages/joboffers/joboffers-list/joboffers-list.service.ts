import { HttpClient } from '@angular/common/http';
import { computed, Inject, Injectable, Resource, signal } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { catchError, map, tap } from 'rxjs';
import { heyerApiUrl } from '../../../app.config';
import { HttpErrorHandlerService } from '../../../http-error-handler.service';
import { ListResponse } from '../../../models/list-response';
import { JobOfferListItem } from './joboffer-list-item';

@Injectable({
  providedIn: 'root',
})
export class JoboffersListService {
  public readonly listSignal: Resource<ListResponse<JobOfferListItem>>;
  public readonly currentPage = signal<number>(1);
  public readonly pageSize = signal<number>(10);

  public readonly url = computed(
    () =>
      `${this.api_url}/job-offers?Page=${this.currentPage().toString()}&PageSize=${this.pageSize().toString()}`,
  );

  constructor(
    private http: HttpClient,
    private errorHandler: HttpErrorHandlerService,
    @Inject(heyerApiUrl) private api_url: string,
  ) {
    this.listSignal = rxResource({
      request: () => ({
        url: this.url(),
      }),
      loader: ({ request }) => this.fetch(request.url),
    });
  }

  private fetch(url: string) {
    return this.http.get<ListResponse<JobOfferListItem>>(url).pipe(
      map(
        (response) =>
          new ListResponse<JobOfferListItem>(
            response.PageSize,
            response.TotalCount,
            response.Items.map((x: JobOfferListItem) =>
              JobOfferListItem.from(x),
            ),
          ),
      ),
      tap((response) => {
        console.log(response);
      }),
      catchError((error: unknown) => {
        this.errorHandler.handleError(error);
        return [];
      }),
    );
  }
}
