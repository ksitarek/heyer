import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { catchError, Observable } from 'rxjs';
import { heyerApiUrl } from '../../../../app.config';
import { HttpErrorHandlerService } from '../../../../http-error-handler.service';

@Injectable({
  providedIn: 'root',
})
export class JobOfferLocationService {
  constructor(
    private http: HttpClient,
    private errorHandler: HttpErrorHandlerService,
    @Inject(heyerApiUrl) private api_url: string,
  ) {}

  public setOfficeLocation(jobOfferId: string, city: string, country: string): Observable<unknown> {
    const payload = {
      jobOfferId,
      city,
      country,
    };

    return this.http.post(`${this.api_url}/job-offers/set-office-location`, payload).pipe(
      catchError((error: unknown) => {
        this.errorHandler.handleError(error);
        return [];
      }),
    );
  }
}
