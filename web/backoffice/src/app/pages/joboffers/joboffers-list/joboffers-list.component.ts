import { JoboffersListService } from './joboffers-list.service';
import { Component, computed, TrackByFunction } from '@angular/core';
import { JobOfferListItem } from './joboffer-list-item';
import { Observable } from 'rxjs';
import { AsyncPipe, NgIf } from '@angular/common';
import { HlmTableModule } from '../../../../../libs/ui/ui-table-helm/src/index';
import { BrnTableModule, useBrnColumnManager } from '@spartan-ng/brain/table';
import { HlmMenuModule } from '@spartan-ng/ui-menu-helm';
import { HlmButtonModule } from '@spartan-ng/ui-button-helm';
import { BrnSelectModule } from '@spartan-ng/brain/select';
import { HlmSelectModule } from '@spartan-ng/ui-select-helm';
import { FormsModule } from '@angular/forms';
import { HDatePipe } from '../../../layout/components/h-date.pipe';
import { JobofferListItemActionsComponent } from './joboffer-list-item-actions/joboffer-list-item-actions.component';
import { PaginationComponent } from '../../../layout/components/pagination/pagination.component';
import { ListResponse } from '../../../models/list-response';

@Component({
  imports: [
    NgIf,
    HDatePipe,
    AsyncPipe,
    FormsModule,
    HlmMenuModule,
    BrnTableModule,
    HlmTableModule,
    HlmButtonModule,
    BrnSelectModule,
    HlmSelectModule,
    JobofferListItemActionsComponent,
    PaginationComponent,
  ],
  selector: 'h-joboffers-list',
  templateUrl: './joboffers-list.component.html',
  styleUrl: './joboffers-list.component.scss',
})
export class JoboffersListComponent {
  public data$!: Observable<ListResponse<JobOfferListItem>>;

  protected readonly columnManager = useBrnColumnManager({
    offerSummary: { visible: true, label: 'Offer Summary' },
    publishedAt: { visible: true, label: 'Published at' },
    publishedUntil: { visible: true, label: 'Published until' },
  });

  protected readonly dataTableColumns = computed(() => [
    'id',
    ...this.columnManager.displayedColumns(),
    'actions',
  ]);

  protected readonly trackBy: TrackByFunction<JobOfferListItem> = (
    _: number,
    p: JobOfferListItem,
  ) => p.Id;

  constructor(public jobOffersListService: JoboffersListService) {
    this.data$ = this.jobOffersListService.getListOfJobs();
  }
}
