import { Component, computed, input } from '@angular/core';
import { RouterModule } from '@angular/router';
import { NgIcon } from '@ng-icons/core';
import { HlmTooltipTriggerDirective } from '@spartan-ng/ui-tooltip-helm';
import { take } from 'rxjs';
import { JobOfferListItem } from '../joboffer-list-item';
import { JobofferActionService } from './joboffer-action.service';

@Component({
  selector: 'h-joboffer-list-item-actions',
  imports: [NgIcon, HlmTooltipTriggerDirective, RouterModule],
  templateUrl: './joboffer-list-item-actions.component.html',
  styleUrl: './joboffer-list-item-actions.component.scss',
})
export class JobofferListItemActionsComponent {
  public readonly item = input.required<JobOfferListItem>();

  public readonly canEdit = computed(() => {
    return !this.item().isPublished;
  });

  public readonly canTakeDown = computed(() => {
    return this.item().isPublished;
  });

  constructor(private jobofferActionService: JobofferActionService) {}

  public takeDown(): void {
    this.jobofferActionService.takeDown(this.item().Id).pipe(take(1)).subscribe();
  }
}
