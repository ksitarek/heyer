import { Component } from '@angular/core';
import { JobOffersListComponent } from './job-offers-list/job-offers-list.component';

@Component({
  selector: 'h-job-board',
  imports: [JobOffersListComponent],
  templateUrl: './job-board.component.html',
  styleUrl: './job-board.component.scss',
})
export class JobBoardComponent {}
