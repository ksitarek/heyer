import { Component } from '@angular/core';
import { SidebarMenuComponent } from './sidebar-menu/sidebar-menu.component';
import { HlmH3Directive } from '@spartan-ng/ui-typography-helm';

@Component({
  selector: 'h-sidebar',
  imports: [SidebarMenuComponent, HlmH3Directive],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.scss',
})
export class SidebarComponent {}
