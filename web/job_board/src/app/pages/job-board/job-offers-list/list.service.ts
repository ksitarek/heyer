import { HttpClient } from '@angular/common/http';
import { computed, Inject, Injectable, ResourceRef, signal } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { catchError, map, Observable } from 'rxjs';
import { heyerApiUrl } from '../../../app.config';
import { HttpErrorHandlerService } from '../../../http-error-handler.service';
import { ListItemModel } from './list-item/list-item.model';

@Injectable({
  providedIn: 'root',
})
export class ListService {
  private readonly listSignal: ResourceRef<ListItemModel[]>;

  private readonly reloadSignal = signal(1);

  public readonly items = computed(() => this.listSignal.value() ?? []);

  constructor(
    private http: HttpClient,
    private errorHandler: HttpErrorHandlerService,
    @Inject(heyerApiUrl) private api_url: string,
  ) {
    this.listSignal = rxResource({
      request: () => ({
        reload: this.reloadSignal(),
      }),

      loader: () => {
        return this.fetch();
      },
    });
  }

  fetch(): Observable<ListItemModel[]> {
    return this.http.get<ListItemModel[]>(`${this.api_url}/job-board`).pipe(
      map((response) => response.map((item) => ListItemModel.from(item))),
      catchError((error) => {
        this.errorHandler.handleError(error);
        return [];
      }),
    );
  }

  public reloadList(): void {
    this.reloadSignal.set(this.reloadSignal() * -1);
  }
}
