import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class LocalStorageService {
  setItem(key: string, value: object) {
    localStorage.setItem(key, JSON.stringify(value));
  }

  getItem(key: string): object {
    return JSON.parse(localStorage.getItem(key) ?? 'null') as object;
  }
}
