import { Routes } from '@angular/router';
import { JobOfferResolver } from './pages/job-offer/job-offer.resolver';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./pages/job-board/job-board.component').then((m) => m.JobBoardComponent),
  },

  {
    path: 'job-board/:id',
    loadComponent: () => import('./pages/job-offer/job-offer.component').then((m) => m.JobOfferComponent),
    resolve: {
      jobOffer: JobOfferResolver,
    },
  },

  {
    path: 'not-found',
    loadComponent: () => import('./pages/not-found/not-found.component').then((m) => m.NotFoundComponent),
  },

  {
    path: '**',
    redirectTo: '/not-found',
  },
];
