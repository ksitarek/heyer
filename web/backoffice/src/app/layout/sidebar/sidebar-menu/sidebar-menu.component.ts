import { Component } from '@angular/core';
import { SidebarMenuItemComponent } from "./sidebar-menu-item/sidebar-menu-item.component";
import { SidebarMenuItemDto } from './sidebar-menu-item-dto';
import { NgFor } from '@angular/common';

@Component({
  selector: 'app-sidebar-menu',
  imports: [SidebarMenuItemComponent, NgFor],
  templateUrl: './sidebar-menu.component.html',
  styleUrl: './sidebar-menu.component.scss'
})
export class SidebarMenuComponent {
  public menuItems: Array<SidebarMenuItemDto> = [
    new SidebarMenuItemDto('Statistics', 'heroPresentationChartLine', '/'),
    new SidebarMenuItemDto('Job Offers', 'heroDocumentCurrencyDollar', '/job-offers'),
    new SidebarMenuItemDto('Candidates Pool', 'heroUserGroup', '/candidates-pool'),
  ];
}

