import { Component } from '@angular/core';
import { SidebarMenuItemComponent } from './sidebar-menu-item/sidebar-menu-item.component';
import { SidebarMenuItemDto } from './sidebar-menu-item-dto';
import { NgFor } from '@angular/common';

@Component({
  selector: 'h-sidebar-menu',
  imports: [SidebarMenuItemComponent, NgFor],
  templateUrl: './sidebar-menu.component.html',
  styleUrl: './sidebar-menu.component.scss',
})
export class SidebarMenuComponent {
  public menuItems: Array<SidebarMenuItemDto> = [
    new SidebarMenuItemDto('Statistics', 'lucideChartNoAxesCombined', '/'),
    new SidebarMenuItemDto('Job Offers', 'lucideMegaphone', '/job-offers'),
    new SidebarMenuItemDto(
      'Candidates Pool',
      'lucideUsers',
      '/candidates-pool'
    ),
  ];
}
