import { Component, OnInit } from '@angular/core';
import { ListService } from '../list.service';
import { Observable } from 'rxjs';
import { ListItemComponent } from "../list-item/list-item.component";
import { AsyncPipe, NgFor, NgForOf } from '@angular/common';

@Component({
  selector: 'app-list',
  imports: [ListItemComponent, AsyncPipe, NgFor],
  templateUrl: './list.component.html',
  styleUrl: './list.component.scss'
})
export class ListComponent implements OnInit {
  public jobs$!: Observable<any>;

  constructor(private listService: ListService) { }

  public ngOnInit(): void {
    this.jobs$ = this.listService.getListOfJobs();
  }
}
