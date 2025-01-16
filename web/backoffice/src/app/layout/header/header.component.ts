import { AsyncPipe, NgIf } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { AuthService, User } from '@auth0/auth0-angular';
import { Observable } from 'rxjs';
import { HeaderUserMenuComponent } from '../components/header-user-menu/header-user-menu.component';

@Component({
  selector: 'h-header',
  imports: [HeaderUserMenuComponent],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss',
})
export class HeaderComponent {}
