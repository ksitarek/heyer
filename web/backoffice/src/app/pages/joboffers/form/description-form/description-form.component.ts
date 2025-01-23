import { Component, input } from '@angular/core';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { HlmInputDirective } from '@spartan-ng/ui-input-helm';
import { HlmLabelDirective } from '@spartan-ng/ui-label-helm';
import { JobOfferForms } from '../joboffer-forms';
import { RemoteWorkControlComponent } from './../../remote-work-control/remote-work-control.component';

@Component({
  selector: 'h-description-form',
  imports: [
    RemoteWorkControlComponent,
    ReactiveFormsModule,
    HlmLabelDirective,
    HlmInputDirective,
  ],
  providers: [JobOfferForms],
  templateUrl: './description-form.component.html',
  styleUrl: './description-form.component.scss',
})
export class DescriptionFormComponent {
  readonly form = input.required<FormGroup>();
}
