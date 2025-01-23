import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { heyerApiUrl } from '../../../app.config';
import { HttpErrorHandlerService } from '../../../http-error-handler.service';
import { catchError, map, Observable, tap } from 'rxjs';
import { JobOfferListItem } from './joboffer-list-item';
import { ListResponse } from '../../../models/list-response';

@Injectable({
  providedIn: 'root',
})
export class JoboffersListService {
  constructor(
    private http: HttpClient,
    private errorHandler: HttpErrorHandlerService,
    @Inject(heyerApiUrl) private api_url: string,
  ) {}

  getListOfJobs(): Observable<ListResponse<JobOfferListItem>> {
    return this.http
      .get<ListResponse<JobOfferListItem>>(`${this.api_url}/job-offers`)
      .pipe(
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
