import { Component } from '@angular/core';
import { HeaderUserMenuComponent } from '../components/header-user-menu/header-user-menu.component';
import { HlmH3Directive } from '@spartan-ng/ui-typography-helm';

@Component({
  selector: 'h-header',
  imports: [HeaderUserMenuComponent, HlmH3Directive],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss',
})
export class HeaderComponent {}
