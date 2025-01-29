import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { catchError, map, Observable, of } from 'rxjs';
import { heyerApiUrl } from '../../../../app.config';
import { HttpErrorHandlerService } from '../../../../http-error-handler.service';

@Injectable({
  providedIn: 'root',
})
export class JobofferActionService {
  constructor(
    private http: HttpClient,
    private errorHandler: HttpErrorHandlerService,
    @Inject(heyerApiUrl) private api_url: string,
  ) {}

  public takeDown(jobOfferId: string): Observable<boolean> {
    const payload = { jobOfferId };
    return this.http.post(`${this.api_url}/job-offers/take-down`, payload).pipe(
      map(() => true),
      catchError((error) => {
        this.errorHandler.handleError(error);
        return of(false);
      }),
    );
  }
}
