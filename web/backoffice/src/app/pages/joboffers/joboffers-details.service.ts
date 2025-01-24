import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { catchError, EMPTY, map, Observable } from 'rxjs';
import { heyerApiUrl } from '../../app.config';
import { HttpErrorHandlerService } from '../../http-error-handler.service';
import { JobOfferDetails } from './joboffer-details';
import { RemoteWork } from './remote-work-control/remote-work';

@Injectable({
  providedIn: 'root',
})
export class JobOfferDetailsService {
  constructor(
    private http: HttpClient,
    private errorHandler: HttpErrorHandlerService,
    @Inject(heyerApiUrl) private api_url: string,
  ) {}

  public getJobOfferDetails(id: string): Observable<JobOfferDetails> {
    return this.http.get<JobOfferDetails>(`${this.api_url}/job-offers/${id}`).pipe(
      map((response) => JobOfferDetails.from(response)),
      catchError((error) => {
        this.errorHandler.handleError(error);
        return EMPTY;
      }),
    );
  }

  public updateJobOfferDescription(
    jobOfferId: string,
    offerSummary: string,
    jobDescription: string,
    remoteWork: RemoteWork,
  ): Observable<unknown> {
    const jobOfferDetails = { jobOfferId, offerSummary, jobDescription, remoteWork };
    return this.http.post(`${this.api_url}/job-offers/update`, jobOfferDetails).pipe(
      catchError((error) => {
        this.errorHandler.handleError(error);
        return EMPTY;
      }),
    );
  }
}
