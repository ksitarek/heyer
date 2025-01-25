import { Component, signal } from '@angular/core';
import { NgIcon } from '@ng-icons/core';
import { HlmButtonDirective } from '@spartan-ng/ui-button-helm';
import { LocalStorageService } from '../../local-storage.service';
import { SidebarMenuComponent } from './sidebar-menu/sidebar-menu.component';

@Component({
  selector: 'h-sidebar',
  imports: [SidebarMenuComponent, HlmButtonDirective, NgIcon],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.scss',
})
export class SidebarComponent {
  protected readonly expanded = signal(true);

  constructor(private localStorageService: LocalStorageService) {
    this.expanded.set(this.localStorageService.getItem('sidebarExpanded') as unknown as boolean);
  }

  public toggle() {
    this.expanded.set(!this.expanded());
    this.localStorageService.setItem('sidebarExpanded', this.expanded() as unknown as object);
  }

  public get sidebarClasses() {
    const baseClasses = 'flex flex-col justify-between';
    const widthClass = this.expanded() ? 'w-64' : 'w-16';

    return `${baseClasses} ${widthClass}`;
  }
}
