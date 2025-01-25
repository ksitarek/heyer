import { Component } from '@angular/core';
import { SidebarMenuItemDto } from './sidebar-menu-item-dto';
import { SidebarMenuItemComponent } from './sidebar-menu-item/sidebar-menu-item.component';

@Component({
  selector: 'h-sidebar-menu',
  imports: [SidebarMenuItemComponent],
  templateUrl: './sidebar-menu.component.html',
  styleUrl: './sidebar-menu.component.scss',
})
export class SidebarMenuComponent {
  public menuItems: SidebarMenuItemDto[] = [
    new SidebarMenuItemDto('Statistics', 'lucideChartNoAxesCombined', '/'),
    new SidebarMenuItemDto('Job Offers', 'lucideMegaphone', '/job-offers'),
    new SidebarMenuItemDto('Candidates Pool', 'lucideUsers', '/candidates-pool'),
  ];
}
