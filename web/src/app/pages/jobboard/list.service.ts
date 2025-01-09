import { HttpClient } from '@angular/common/http';
import { Inject, Injectable, InjectionToken } from '@angular/core';
import { catchError, map, Observable, tap } from 'rxjs';
import { heyerApiUrl } from '../../app.config';
import { ListItemModel, RemoteWork } from './list-item.model';
import { HttpErrorHandlerService } from '../../http-error-handler.service';

@Injectable({
  providedIn: 'root',
})
export class ListService {
  constructor(
    private http: HttpClient,
    private errorHandler: HttpErrorHandlerService,
    @Inject(heyerApiUrl) private api_url: string
  ) {}

  getListOfJobs(): Observable<ListItemModel[]> {
    return this.http.get<ListItemModel[]>(`${this.api_url}/job-board`).pipe(
      map((response) => response.map((item) =>  ListItemModel.from(item))),
      catchError((error) => {
        this.errorHandler.handleError(error);
        return [];
      }));
  }
}
