import { Routes } from '@angular/router';
import { JobOfferResolver } from './pages/joboffers/joboffer-resolver';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./pages/statistics/statistics.component').then(
        (m) => m.StatisticsComponent,
      ),
  },
  {
    path: 'job-offers',
    loadComponent: () =>
      import('./pages/joboffers/joboffer.component').then(
        (m) => m.JoboffersComponent,
      ),
  },
  {
    path: 'job-offers/create',
    loadComponent: () =>
      import(
        './pages/joboffers/create-joboffer/create-joboffer.component'
      ).then((m) => m.CreateJobofferComponent),
  },
  {
    path: 'job-offers/edit/:id',
    resolve: {
      jobOffer: JobOfferResolver,
    },
    loadComponent: () =>
      import('./pages/joboffers/edit-joboffer/edit-joboffer.component').then(
        (m) => m.EditJobofferComponent,
      ),
  },
];
