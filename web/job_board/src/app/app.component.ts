import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { environment } from '../environments/environment';
import { JsonPipe } from '@angular/common';
import { ListComponent } from "./pages/jobboard/list/list.component";
import { MainComponent } from "./layout/main/main.component";

@Component({
  selector: 'app-root',
  imports: [MainComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'Heyer';
  env = environment;
}
