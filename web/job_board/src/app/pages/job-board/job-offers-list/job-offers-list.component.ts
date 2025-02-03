import { Component, inject, OnInit } from '@angular/core';
import { ListItemComponent } from './list-item/list-item.component';
import { ListService } from './list.service';

@Component({
  selector: 'h-job-offers-list',
  imports: [ListItemComponent],
  templateUrl: './job-offers-list.component.html',
  styleUrl: './job-offers-list.component.scss',
})
export class JobOffersListComponent implements OnInit {
  private readonly listService = inject(ListService);

  public readonly items = this.listService.items;

  public ngOnInit(): void {
    this.reload();
  }

  public reload() {
    this.listService.reloadList();
  }
}
