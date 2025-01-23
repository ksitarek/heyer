import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { heyerApiUrl } from '../../../app.config';
import { HttpErrorHandlerService } from '../../../http-error-handler.service';
import { RemoteWork } from '../remote-work-control/remote-work';
import { catchError, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class CreateJobOfferService {
  constructor(
    private http: HttpClient,
    private errorHandler: HttpErrorHandlerService,
    @Inject(heyerApiUrl) private api_url: string
  ) {}

  public saveDraft(
    offerSummary: string,
    jobDescription: string,
    remoteWork: RemoteWork
  ): Observable<string> {
    return this.http
      .post<string>(`${this.api_url}/job-offers/create`, {
        offerSummary,
        jobDescription,
        remoteWork,
      })
      .pipe(
        catchError((error) => {
          this.errorHandler.handleError(error);
          return '';
        })
      );
  }
}
