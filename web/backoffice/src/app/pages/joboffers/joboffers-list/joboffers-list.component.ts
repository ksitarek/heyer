import { Component } from '@angular/core';
import { DataTableColumn } from '../../../layout/components/data-table/data-table-column';
import { DataTableComponent } from '../../../layout/components/data-table/data-table.component';
import { JobOfferListItem } from './joboffer-list-item';
import { Observable, of } from 'rxjs';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'h-joboffers-list',
  imports: [DataTableComponent, DatePipe],
  templateUrl: './joboffers-list.component.html',
  styleUrl: './joboffers-list.component.scss',
})
export class JoboffersListComponent {
  public dataTableColumns: DataTableColumn[] = [
    new DataTableColumn('id', ''),
    new DataTableColumn('offerSummary', 'Offer Summary'),
    new DataTableColumn('publishedAt', 'Published at'),
    new DataTableColumn('publishedUntil', 'Published until'),
    new DataTableColumn('actions', ''),
  ];

  public data$: Observable<JobOfferListItem[]> = of([
    new JobOfferListItem(
      '1',
      'Job Offer 1',
      new Date(),
      new Date(),
      'Actions 1'
    ),
    new JobOfferListItem(
      '2',
      'Job Offer 2',
      new Date(),
      new Date(),
      'Actions 2'
    ),
    new JobOfferListItem(
      '3',
      'Job Offer 3',
      new Date(),
      new Date(),
      'Actions 3'
    ),
  ]);
}
