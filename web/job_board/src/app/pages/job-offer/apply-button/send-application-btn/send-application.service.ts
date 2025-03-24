import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { catchError, map, Observable } from 'rxjs';
import { heyerApiUrl } from '../../../../app.config';
import { HttpErrorHandlerService } from '../../../../http-error-handler.service';
import { NewCandidateApplication } from './new-candidate-application.model';

@Injectable({
  providedIn: 'root',
})
export class SendApplicationService {
  constructor(
    private http: HttpClient,
    private errorHandler: HttpErrorHandlerService,
    @Inject(heyerApiUrl) private api_url: string,
  ) {}

  public newCandidateApply(payload: NewCandidateApplication): Observable<boolean> {
    return this.http.post(`${this.api_url}/job-offers/new-candidate-apply`, payload).pipe(
      map(() => true),
      catchError((error) => {
        this.errorHandler.handleError(error);
        return [false];
      }),
    );
  }
}
