import { Component, input } from '@angular/core';
import { RouterModule } from '@angular/router';
import { NgIcon } from '@ng-icons/core';
import { HlmButtonDirective } from '@spartan-ng/ui-button-helm';
import { HlmTooltipTriggerDirective } from '@spartan-ng/ui-tooltip-helm';
import { SidebarMenuItemDto } from '../sidebar-menu-item-dto';

@Component({
  selector: 'h-sidebar-menu-item',
  imports: [NgIcon, RouterModule, HlmButtonDirective, HlmTooltipTriggerDirective],
  providers: [],
  templateUrl: './sidebar-menu-item.component.html',
  styleUrl: './sidebar-menu-item.component.scss',
})
export class SidebarMenuItemComponent {
  public readonly expanded = input.required<boolean>();

  public readonly dto = input.required<SidebarMenuItemDto>();
}
