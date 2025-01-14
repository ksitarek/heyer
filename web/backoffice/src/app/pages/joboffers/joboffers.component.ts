import { Component } from '@angular/core';
import { PageHeaderComponent } from '../../layout/components/page-header/page-header.component';
import { JoboffersListComponent } from './joboffers-list/joboffers-list.component';

@Component({
  selector: 'h-joboffers',
  imports: [PageHeaderComponent, JoboffersListComponent],
  templateUrl: './joboffers.component.html',
  styleUrl: './joboffers.component.scss',
})
export class JoboffersComponent {}
