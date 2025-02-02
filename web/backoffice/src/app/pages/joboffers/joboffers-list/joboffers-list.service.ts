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
  public readonly sortBy = signal('PublishedAt');
  public readonly sortOrder = signal('asc');

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
        sortBy: this.sortBy(),
        sortOrder: this.sortOrder(),
        reload: this.reloadSignal(),
      }),

      loader: ({ request }) => {
        return this.fetch(request.page, request.pageSize, request.sortBy, request.sortOrder);
      },
    });
  }

  private fetch(
    currentPage: number,
    pageSize: number,
    sortBy: string,
    sortOrder: string,
  ): Observable<ListResponse<JobOfferListItem>> {
    const url = `${this.api_url}/job-offers?Page=${currentPage.toString()}&PageSize=${pageSize.toString()}&SortBy=${sortBy}&SortOrder=${sortOrder}`;

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
