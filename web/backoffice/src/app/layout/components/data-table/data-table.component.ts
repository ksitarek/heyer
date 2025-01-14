import { Component, ContentChild, Input, TemplateRef } from '@angular/core';
import { DataTableColumn } from './data-table-column';
import { AsyncPipe, NgFor, NgTemplateOutlet } from '@angular/common';
import { Observable } from 'rxjs';

@Component({
  selector: 'h-data-table',
  imports: [NgFor, AsyncPipe, NgTemplateOutlet],
  templateUrl: './data-table.component.html',
  styleUrl: './data-table.component.scss',
})
export class DataTableComponent {
  @Input({ required: true }) columns!: DataTableColumn[];
  @Input({ required: true }) data$!: Observable<any>;

  @ContentChild('row') rowTemplate!: TemplateRef<any>;
}
