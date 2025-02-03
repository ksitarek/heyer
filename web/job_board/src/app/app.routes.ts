import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./pages/job-board/job-board.component').then((m) => m.JobBoardComponent),
  },
];
