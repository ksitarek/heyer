import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { heyerApiUrl } from '../../../app.config';
import { HttpErrorHandlerService } from '../../../http-error-handler.service';
import { catchError, map, Observable, tap } from 'rxjs';
import { JobOfferListItem } from './joboffer-list-item';

@Injectable({
  providedIn: 'root',
})
export class JoboffersListService {
  constructor(
    private http: HttpClient,
    private errorHandler: HttpErrorHandlerService,
    @Inject(heyerApiUrl) private api_url: string
  ) {}

  getListOfJobs(): Observable<JobOfferListItem[]> {
    return this.http.get<JobOfferListItem[]>(`${this.api_url}/job-offers`).pipe(
      map((response) => response.map((item) => JobOfferListItem.from(item))),
      tap((response) => console.log(response)),
      catchError((error) => {
        this.errorHandler.handleError(error);
        return [];
      })
    );
  }
}
