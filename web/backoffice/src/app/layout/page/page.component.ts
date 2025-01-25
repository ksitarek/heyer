import { AsyncPipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AuthService } from '@auth0/auth0-angular';
import { Observable } from 'rxjs';
import { FooterComponent } from '../footer/footer.component';
import { HeaderComponent } from '../header/header.component';
import { SidebarComponent } from '../sidebar/sidebar.component';

@Component({
  selector: 'h-page',
  imports: [AsyncPipe, HeaderComponent, FooterComponent, RouterOutlet, SidebarComponent],
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
