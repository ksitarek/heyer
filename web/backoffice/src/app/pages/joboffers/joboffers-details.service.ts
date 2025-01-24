import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { catchError, EMPTY, map, Observable } from 'rxjs';
import { heyerApiUrl } from '../../app.config';
import { HttpErrorHandlerService } from '../../http-error-handler.service';
import { ContractDetails, EmploymentType, JobOfferDetails } from './joboffer-details';
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

  public addContractDetails(jobOfferId: string, contractDetails: ContractDetails) {
    const payload = { jobOfferId, contractDetails };
    return this.http.post(`${this.api_url}/job-offers/add-contract-details`, payload).pipe(
      catchError((error) => {
        this.errorHandler.handleError(error);
        return EMPTY;
      }),
    );
  }

  public removeContractDetails(jobOfferId: string, employmentType: EmploymentType) {
    return this.http.post(`${this.api_url}/job-offers/remove-contract-details`, { jobOfferId, employmentType }).pipe(
      catchError((error) => {
        this.errorHandler.handleError(error);
        return EMPTY;
      }),
    );
  }

  public updateContractDetails(jobOfferId: string, contractDetails: ContractDetails) {
    const payload = {
      jobOfferId,
      employmentType: contractDetails.EmploymentType,
      salaryRange: contractDetails.SalaryRange,
      timeNumerator: 8,
      timeDenominator: 8,
    };
    return this.http.post(`${this.api_url}/job-offers/update-contract-details`, payload).pipe(
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
