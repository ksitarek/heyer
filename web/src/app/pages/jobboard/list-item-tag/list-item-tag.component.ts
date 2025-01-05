import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-list-item-tag',
  imports: [],
  templateUrl: './list-item-tag.component.html',
  styleUrl: './list-item-tag.component.scss'
})
export class ListItemTagComponent {
  @Input({required: true}) tag!: string;
}
