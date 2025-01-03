import { Component, Input } from '@angular/core';
import { ListItemModel } from '../list-item.model';
import { JsonPipe } from '@angular/common';

@Component({
  selector: 'app-list-item',
  imports: [JsonPipe],
  templateUrl: './list-item.component.html',
  styleUrl: './list-item.component.scss'
})
export class ListItemComponent {
  @Input({required: true}) item!: ListItemModel;
}
