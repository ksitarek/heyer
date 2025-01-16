import { AsyncPipe, NgIf } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { AuthService, User } from '@auth0/auth0-angular';
import { NgIcon } from '@ng-icons/core';
import { BrnMenuTriggerDirective } from '@spartan-ng/brain/menu';
import {
  HlmMenuComponent,
  HlmMenuGroupComponent,
  HlmMenuItemDirective,
  HlmMenuItemIconDirective,
  HlmMenuLabelComponent,
  HlmMenuSeparatorComponent,
} from '@spartan-ng/ui-menu-helm';
import { Observable } from 'rxjs';

@Component({
  selector: 'h-header-user-menu',
  imports: [
    NgIf,
    AsyncPipe,
    NgIcon,
    BrnMenuTriggerDirective,

    HlmMenuLabelComponent,
    HlmMenuSeparatorComponent,
    HlmMenuGroupComponent,
    HlmMenuComponent,
    HlmMenuItemDirective,
    HlmMenuItemIconDirective,
  ],
  templateUrl: './header-user-menu.component.html',
  styleUrl: './header-user-menu.component.scss',
})
export class HeaderUserMenuComponent implements OnInit {
  constructor(private authService: AuthService) {}

  public ngOnInit(): void {}

  public logout(): void {
    this.authService.logout();
  }

  public get user$(): Observable<User | null | undefined> {
    return this.authService.user$;
  }
}
