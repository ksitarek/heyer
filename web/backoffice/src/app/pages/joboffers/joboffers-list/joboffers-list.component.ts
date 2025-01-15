import { JoboffersListService } from './joboffers-list.service';
import { Component, OnInit } from '@angular/core';
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
export class JoboffersListComponent implements OnInit {
  public dataTableColumns: DataTableColumn[] = [
    new DataTableColumn('id', ''),
    new DataTableColumn('offerSummary', 'Offer Summary'),
    new DataTableColumn('publishedAt', 'Published at'),
    new DataTableColumn('publishedUntil', 'Published until'),
    new DataTableColumn('actions', ''),
  ];

  public data$!: Observable<JobOfferListItem[]>;

  constructor(public jobOffersListService: JoboffersListService) {}

  public ngOnInit(): void {
    this.data$ = this.jobOffersListService.getListOfJobs();
  }
}
