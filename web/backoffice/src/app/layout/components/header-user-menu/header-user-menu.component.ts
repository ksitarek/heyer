import { AsyncPipe, NgIf } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { AuthService, User } from '@auth0/auth0-angular';
import { Observable } from 'rxjs';
import { HeaderUserMenuItemComponent } from '../header-user-menu-item/header-user-menu-item.component';

@Component({
  selector: 'h-header-user-menu',
  imports: [NgIf, AsyncPipe, HeaderUserMenuItemComponent],
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
