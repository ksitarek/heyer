import { Component, EventEmitter, Input, Output } from '@angular/core';
import { NgIcon } from '@ng-icons/core';

@Component({
  selector: 'h-header-user-menu-item',
  imports: [NgIcon],
  templateUrl: './header-user-menu-item.component.html',
  styleUrl: './header-user-menu-item.component.scss',
})
export class HeaderUserMenuItemComponent {
  @Input({ required: true }) public icon!: string;
  @Input({ required: true }) public label!: string;

  @Output() public onItemClicked = new EventEmitter<void>();

  public itemClicked(): void {
    this.onItemClicked.emit();
  }
}
