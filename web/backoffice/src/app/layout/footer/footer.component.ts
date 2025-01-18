import { Component } from '@angular/core';
import {
  HlmMutedDirective,
  HlmSmallDirective,
} from '@spartan-ng/ui-typography-helm';

@Component({
  selector: 'h-footer',
  imports: [HlmMutedDirective, HlmSmallDirective],
  templateUrl: './footer.component.html',
  styleUrl: './footer.component.scss',
})
export class FooterComponent {}
