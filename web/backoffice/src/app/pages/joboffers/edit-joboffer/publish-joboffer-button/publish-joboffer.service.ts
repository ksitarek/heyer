import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { catchError, map, Observable, of } from 'rxjs';
import { heyerApiUrl } from '../../../../app.config';
import { HttpErrorHandlerService } from '../../../../http-error-handler.service';

@Injectable({
  providedIn: 'root',
})
export class PublishJobofferService {
  constructor(
    private http: HttpClient,
    private errorHandler: HttpErrorHandlerService,
    @Inject(heyerApiUrl) private api_url: string,
  ) {}

  public checkForConflicts(id: string): Observable<boolean> {
    return this.http.get<boolean>(`${this.api_url}/job-offers/${id}/check-for-conflicts`).pipe(
      catchError((error) => {
        this.errorHandler.handleError(error);
        return of(false);
      }),
    );
  }

  public publish(jobOfferId: string): Observable<boolean> {
    const payload = {
      jobOfferId,
    };

    return this.http.post(`${this.api_url}/job-offers/publish`, payload).pipe(
      map(() => true),
      catchError((error) => {
        this.errorHandler.handleError(error);
        return of(false);
      }),
    );
  }
}
