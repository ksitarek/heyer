import { Component, signal } from '@angular/core';
import { NgIcon } from '@ng-icons/core';
import { HlmButtonDirective } from '@spartan-ng/ui-button-helm';
import { SidebarMenuComponent } from './sidebar-menu/sidebar-menu.component';

@Component({
  selector: 'h-sidebar',
  imports: [SidebarMenuComponent, HlmButtonDirective, NgIcon],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.scss',
})
export class SidebarComponent {
  protected readonly expanded = signal(true);

  public toggle() {
    this.expanded.set(!this.expanded());
  }
}
