import { HttpClient } from '@angular/common/http';
import { computed, Inject, Injectable, ResourceRef, signal } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { catchError, map, Observable } from 'rxjs';
import { heyerApiUrl } from '../../../app.config';
import { HttpErrorHandlerService } from '../../../http-error-handler.service';
import { ListResponse } from '../../../models/list-response';
import { JobOfferListItem } from './joboffer-list-item';

@Injectable({
  providedIn: 'root',
})
export class JoboffersListService {
  private readonly listSignal: ResourceRef<ListResponse<JobOfferListItem>>;

  public readonly page = signal(1);
  public readonly pageSize = signal(10);

  private readonly reloadSignal = signal(1);

  public readonly items = computed(() => this.listSignal.value()?.Items ?? []);
  public readonly totalCount = computed(() => this.listSignal.value()?.TotalCount ?? 0);

  constructor(
    private http: HttpClient,
    private errorHandler: HttpErrorHandlerService,
    @Inject(heyerApiUrl) private api_url: string,
  ) {
    this.listSignal = rxResource({
      request: () => ({
        page: this.page(),
        pageSize: this.pageSize(),
        reload: this.reloadSignal(),
      }),

      loader: ({ request }) => {
        return this.fetch(request.page, request.pageSize);
      },
    });
  }

  private fetch(currentPage: number, pageSize: number): Observable<ListResponse<JobOfferListItem>> {
    const url = `${this.api_url}/job-offers?Page=${currentPage.toString()}&PageSize=${pageSize.toString()}`;

    return this.http.get<ListResponse<JobOfferListItem>>(url).pipe(
      map(
        (response) =>
          new ListResponse<JobOfferListItem>(
            response.PageSize,
            response.TotalCount,
            response.Items.map((x: JobOfferListItem) => JobOfferListItem.from(x)),
          ),
      ),
      catchError((error: unknown) => {
        this.errorHandler.handleError(error);
        return [];
      }),
    );
  }

  public reloadList(): void {
    this.reloadSignal.set(this.reloadSignal() * -1);
  }
}
