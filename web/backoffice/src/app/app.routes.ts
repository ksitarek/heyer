import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./pages/statistics/statistics.component').then(m => m.StatisticsComponent),
  },
  {
    path: 'job-offers',
    loadComponent: () => import('./pages/joboffers/joboffers.component').then(m => m.JoboffersComponent),
  }
];
