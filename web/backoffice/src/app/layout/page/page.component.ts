import { AsyncPipe, NgIf } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { HeaderComponent } from '../header/header.component';
import { FooterComponent } from '../footer/footer.component';
import { RouterOutlet } from '@angular/router';
import { SidebarComponent } from '../sidebar/sidebar.component';
import { AuthService } from '@auth0/auth0-angular';
import { Observable } from 'rxjs';

@Component({
  selector: 'h-page',
  imports: [
    NgIf,
    AsyncPipe,
    HeaderComponent,
    FooterComponent,
    RouterOutlet,
    SidebarComponent,
  ],
  templateUrl: './page.component.html',
  styleUrl: './page.component.scss',
})
export class PageComponent implements OnInit {
  constructor(private authService: AuthService) {}

  public ngOnInit(): void {
    this.authService.isAuthenticated$.subscribe((isAuthenticated) => {
      if (!isAuthenticated) {
        this.authService.loginWithRedirect();
      }
    });
  }

  public get isAuthenticated$(): Observable<boolean> {
    return this.authService.isAuthenticated$;
  }
}
