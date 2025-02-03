import { ApplicationConfig, InjectionToken, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { provideHttpClient, withFetch } from '@angular/common/http';
import { provideClientHydration, withEventReplay } from '@angular/platform-browser';
import { provideIcons } from '@ng-icons/core';
import { routes } from './app.routes';

import * as lucide from '@ng-icons/lucide';
import { environment } from '../environments/environment';

export const heyerApiUrl = new InjectionToken<string>('heyerApiUrl');

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideClientHydration(withEventReplay()),
    provideHttpClient(withFetch()),
    provideIcons(lucide),
    { provide: heyerApiUrl, useValue: environment.heyerApi },
  ],
};
