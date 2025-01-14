import { Component, Input } from '@angular/core';
import { SidebarMenuItemDto } from '../sidebar-menu-item-dto';
import { NgIcon } from '@ng-icons/core';
import { RouterModule } from '@angular/router';


@Component({
  selector: 'app-sidebar-menu-item',
  imports: [NgIcon, RouterModule],
  providers: [],
  templateUrl: './sidebar-menu-item.component.html',
  styleUrl: './sidebar-menu-item.component.scss'
})
export class SidebarMenuItemComponent {
   @Input({ required: true }) dto!: SidebarMenuItemDto;
}
