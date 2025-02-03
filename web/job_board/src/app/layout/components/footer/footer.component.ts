import { Component } from '@angular/core';
import { HlmH3Directive } from './../../../../../libs/ui/ui-typography-helm/src/lib/hlm-h3.directive';

@Component({
  selector: 'h-footer',
  imports: [HlmH3Directive],
  templateUrl: './footer.component.html',
  styleUrl: './footer.component.scss',
})
export class FooterComponent {}
