import { JoboffersListService } from './joboffers-list.service';
import { Component, computed, OnInit, TrackByFunction } from '@angular/core';
import { JobOfferListItem } from './joboffer-list-item';
import { Observable } from 'rxjs';
import { DatePipe } from '@angular/common';
import { HlmTableModule } from '../../../../../libs/ui/ui-table-helm/src/index';
import { BrnTableModule, useBrnColumnManager } from '@spartan-ng/brain/table';
import { HlmMenuModule } from '@spartan-ng/ui-menu-helm';
import { HlmButtonModule } from '@spartan-ng/ui-button-helm';
import { BrnSelectModule } from '@spartan-ng/brain/select';
import { HlmSelectModule } from '@spartan-ng/ui-select-helm';
import { FormsModule } from '@angular/forms';
import { HDatePipe } from '../../../layout/components/h-date.pipe';
import { HlmIconDirective } from '../../../../../libs/ui/ui-icon-helm/src/lib/hlm-icon.directive';
import { NgIcon } from '@ng-icons/core';

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
    HlmIconDirective,
    NgIcon,
  ],
  selector: 'h-joboffers-list',
  templateUrl: './joboffers-list.component.html',
  styleUrl: './joboffers-list.component.scss',
})
export class JoboffersListComponent {
  public data$!: Observable<JobOfferListItem[]>;

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
    p: JobOfferListItem
  ) => p.id;

  constructor(public jobOffersListService: JoboffersListService) {
    this.data$ = this.jobOffersListService.getListOfJobs();
  }
}
