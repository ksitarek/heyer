import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { JobOfferForms } from '../joboffer-forms';
import { RemoteWorkControlComponent } from './../../remote-work-control/remote-work-control.component';
import { Component, inject, Input } from '@angular/core';
import { HlmLabelDirective } from '@spartan-ng/ui-label-helm';
import { HlmInputDirective } from '@spartan-ng/ui-input-helm';

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
  @Input({ required: true }) form!: FormGroup;
}
