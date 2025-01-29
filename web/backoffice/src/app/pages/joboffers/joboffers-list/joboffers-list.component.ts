import { Component, computed, inject, OnInit, TrackByFunction } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrnSelectModule } from '@spartan-ng/brain/select';
import { BrnTableModule, useBrnColumnManager } from '@spartan-ng/brain/table';
import { HlmButtonModule } from '@spartan-ng/ui-button-helm';
import { HlmMenuModule } from '@spartan-ng/ui-menu-helm';
import { HlmSelectModule } from '@spartan-ng/ui-select-helm';
import { HlmTableModule } from '../../../../../libs/ui/ui-table-helm/src/index';
import { HDatePipe } from '../../../layout/components/h-date.pipe';
import { PaginationComponent } from '../../../layout/components/pagination/pagination.component';
import { JobOfferListItem } from './joboffer-list-item';
import { JobofferListItemActionsComponent } from './joboffer-list-item-actions/joboffer-list-item-actions.component';
import { JoboffersListService } from './joboffers-list.service';

@Component({
  imports: [
    HDatePipe,
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
export class JoboffersListComponent implements OnInit {
  private readonly jobOffersListService = inject(JoboffersListService);

  public readonly items = this.jobOffersListService.items;

  public readonly totalCount = this.jobOffersListService.totalCount;

  protected readonly columnManager = useBrnColumnManager({
    offerSummary: { visible: true, label: 'Offer Summary' },
    publishedAt: { visible: true, label: 'Published at' },
    publishedUntil: { visible: true, label: 'Published until' },
  });

  protected readonly dataTableColumns = computed(() => ['id', ...this.columnManager.displayedColumns(), 'actions']);

  protected readonly trackBy: TrackByFunction<JobOfferListItem> = (_: number, p: JobOfferListItem) => p.Id;

  public ngOnInit(): void {
    this.jobOffersListService.reloadList();
  }

  public get currentPage() {
    return this.jobOffersListService.page;
  }

  public get pageSize() {
    return this.jobOffersListService.pageSize;
  }

  public getRowClass(item: JobOfferListItem): string {
    if (item.isPublished) {
      return 'bg-sky-50';
    } else if (item.wasPublished) {
      return 'bg-slate-100';
    } else {
      return 'bg-transparent';
    }
  }

  public reload() {
    this.jobOffersListService.reloadList();
  }
}
