import { PageHeaderComponent } from './../../layout/components/page-header/page-header.component';
import { Component } from '@angular/core';
import { ActiveJobOffersTileComponent } from './active-job-offers-tile/active-job-offers-tile.component';
import { NgFor } from '@angular/common';

@Component({
  selector: 'app-statistics',
  imports: [PageHeaderComponent, ActiveJobOffersTileComponent, NgFor],
  templateUrl: './statistics.component.html',
  styleUrl: './statistics.component.scss',
})
export class StatisticsComponent {}
