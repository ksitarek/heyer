import { Component, input } from '@angular/core';
import { RouterModule } from '@angular/router';
import { NgIcon } from '@ng-icons/core';
import { SidebarMenuItemDto } from '../sidebar-menu-item-dto';

@Component({
  selector: 'h-sidebar-menu-item',
  imports: [NgIcon, RouterModule],
  providers: [],
  templateUrl: './sidebar-menu-item.component.html',
  styleUrl: './sidebar-menu-item.component.scss',
})
export class SidebarMenuItemComponent {
  protected readonly dto = input.required<SidebarMenuItemDto>();
}
