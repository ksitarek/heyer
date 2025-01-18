import { Component, Input } from '@angular/core';
import { JobOfferListItem } from '../joboffer-list-item';
import { NgIcon } from '@ng-icons/core';
import { HlmTooltipTriggerDirective } from '@spartan-ng/ui-tooltip-helm';

@Component({
  selector: 'h-joboffer-list-item-actions',
  imports: [NgIcon, HlmTooltipTriggerDirective],
  templateUrl: './joboffer-list-item-actions.component.html',
  styleUrl: './joboffer-list-item-actions.component.scss',
})
export class JobofferListItemActionsComponent {
  @Input({ required: true }) public item!: JobOfferListItem;
}
