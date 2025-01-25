import { Component } from '@angular/core';
import { PageHeaderComponent } from '../../layout/components/page-header/page-header.component';
import { JoboffersListComponent } from './joboffers-list/joboffers-list.component';
import { HlmButtonDirective } from '@spartan-ng/ui-button-helm';
import { NgIcon } from '@ng-icons/core';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'h-joboffers',
  imports: [PageHeaderComponent, JoboffersListComponent, NgIcon, HlmButtonDirective, RouterModule],
  templateUrl: './joboffers.component.html',
  styleUrl: './joboffers.component.scss',
})
export class JoboffersComponent {}
