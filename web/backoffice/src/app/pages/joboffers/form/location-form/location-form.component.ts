import { Component, input } from '@angular/core';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { HlmInputDirective } from '@spartan-ng/ui-input-helm';
import { HlmLabelDirective } from '@spartan-ng/ui-label-helm';

@Component({
  selector: 'h-location-form',
  imports: [ReactiveFormsModule, HlmLabelDirective, HlmInputDirective],
  templateUrl: './location-form.component.html',
  styleUrl: './location-form.component.scss',
})
export class LocationFormComponent {
  readonly form = input.required<FormGroup>();
}
