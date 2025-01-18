import { Component } from '@angular/core';
import { HlmH1Directive } from '@spartan-ng/ui-typography-helm';

@Component({
  selector: 'h-page-header',
  imports: [HlmH1Directive],
  templateUrl: './page-header.component.html',
  styleUrl: './page-header.component.scss',
})
export class PageHeaderComponent {}
