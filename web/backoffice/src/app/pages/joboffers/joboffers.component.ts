import { Component } from '@angular/core';
import { PageHeaderComponent } from '../../layout/components/page-header/page-header.component';

@Component({
  selector: 'app-joboffers',
  imports: [PageHeaderComponent],
  templateUrl: './joboffers.component.html',
  styleUrl: './joboffers.component.scss',
})
export class JoboffersComponent {
  public dataTableColumns = [''];
}
