import { Component, signal } from '@angular/core';
import { SidebarMenuComponent } from './sidebar-menu/sidebar-menu.component';
import { HlmButtonDirective } from '@spartan-ng/ui-button-helm';
import { NgIcon } from '@ng-icons/core';
import { NgIf } from '@angular/common';

@Component({
  selector: 'h-sidebar',
  imports: [SidebarMenuComponent, HlmButtonDirective, NgIcon, NgIf],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.scss',
})
export class SidebarComponent {
  protected readonly expanded = signal(true);

  public toggle() {
    this.expanded.set(!this.expanded());
  }
}
