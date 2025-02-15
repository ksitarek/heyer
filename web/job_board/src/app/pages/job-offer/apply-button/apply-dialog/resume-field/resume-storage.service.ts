import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { catchError, EMPTY, map, Observable, switchMap } from 'rxjs';
import { storageApiUrl } from '../../../../../app.config';
import { HttpErrorHandlerService } from '../../../../../http-error-handler.service';
import { StoreResult } from './store-result.model';

@Injectable({
  providedIn: 'root',
})
export class ResumeStorageService {
  constructor(
    private http: HttpClient,
    private errorHandler: HttpErrorHandlerService,
    @Inject(storageApiUrl) private api_url: string,
  ) {}

  public store(file: File): Observable<StoreResult> {
    const formData = new FormData();
    formData.append('File', file);

    return this.http.get(`${this.api_url}/csrf`, { responseType: 'text', withCredentials: true }).pipe(
      switchMap((csrf) => {
        const headers = new HttpHeaders().set('X-XSRF-TOKEN', csrf);
        return this.http
          .post<StoreResult>(`${this.api_url}/store`, formData, { headers, withCredentials: true })
          .pipe(map((storeResult) => StoreResult.from(storeResult)));
      }),
      catchError((error) => {
        this.errorHandler.handleError(error);
        return EMPTY;
      }),
    );
  }
}
