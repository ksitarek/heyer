import {
  ApplicationConfig,
  InjectionToken,
  provideZoneChangeDetection,
} from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import {
  provideClientHydration,
  withEventReplay,
} from '@angular/platform-browser';
import {
  provideHttpClient,
  withFetch,
  withInterceptors,
} from '@angular/common/http';
import { provideIcons } from '@ng-icons/core';

import * as lucide from '@ng-icons/lucide';
import { environment } from '../environments/environment';
import { provideAuth0 } from '@auth0/auth0-angular';

import { authHttpInterceptorFn } from '@auth0/auth0-angular';

export const heyerApiUrl = new InjectionToken<string>('heyerApiUrl');

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideClientHydration(withEventReplay()),
    provideHttpClient(withFetch(), withInterceptors([authHttpInterceptorFn])),
    provideIcons(lucide),
    provideAuth0({
      domain: 'vtb.eu.auth0.com',
      clientId: 'nzt4gROsplg8llThJS4ft4Hl0eJ9NsUQ',
      authorizationParams: {
        redirect_uri: window.location.origin,
        audience: 'http://heyer',
      },

      httpInterceptor: {
        allowedList: [
          {
            uri: `${environment.heyerApi}/*`,
          },
        ],
      },
    }),
    { provide: heyerApiUrl, useValue: environment.heyerApi },
  ],
};
