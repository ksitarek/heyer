import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class HttpErrorHandlerService {
  public handleError(error: HttpErrorResponse): void {
    console.error('An error occurred:', error);
  }
}
