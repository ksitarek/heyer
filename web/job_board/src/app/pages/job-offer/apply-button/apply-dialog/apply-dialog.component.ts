import { Component, inject, InputSignal } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { BrnDialogRef, injectBrnDialogContext } from '@spartan-ng/brain/dialog';
import { HlmCheckboxModule } from '@spartan-ng/ui-checkbox-helm';
import { HlmDialogTitleDirective } from '@spartan-ng/ui-dialog-helm';
import { HlmInputDirective } from '@spartan-ng/ui-input-helm';
import { HlmLabelDirective } from '@spartan-ng/ui-label-helm';
import { HlmH2Directive } from '@spartan-ng/ui-typography-helm';
import { HlmDialogHeaderComponent } from '../../../../../../libs/ui/ui-dialog-helm/src/lib/hlm-dialog-header.component';
import { HlmFormFieldComponent } from '../../../../../../libs/ui/ui-formfield-helm/src/lib/hlm-form-field.component';
import { JobOfferDetails } from '../../job-offer-details.model';
import { SendApplicationBtnComponent } from '../send-application-btn/send-application-btn.component';
import { ApplyForm } from './apply.form';
import { ResumeFieldComponent } from './resume-field/resume-field.component';

@Component({
  selector: 'h-apply-dialog',
  imports: [
    HlmH2Directive,
    HlmDialogTitleDirective,
    HlmDialogHeaderComponent,
    HlmInputDirective,
    HlmFormFieldComponent,
    HlmLabelDirective,
    ReactiveFormsModule,
    HlmCheckboxModule,
    SendApplicationBtnComponent,
    ResumeFieldComponent,
  ],
  templateUrl: './apply-dialog.component.html',
  styleUrl: './apply-dialog.component.scss',
})
export class ApplyDialogComponent {
  private readonly dialogRef = inject<BrnDialogRef>(BrnDialogRef);
  private readonly dialogContext = injectBrnDialogContext<{ details: InputSignal<JobOfferDetails> }>();

  public readonly applyForm = inject(ApplyForm).form;

  public get details(): JobOfferDetails {
    return this.dialogContext.details();
  }

  public get offerSummary(): string {
    return this.details.OfferSummary;
  }

  public get companyName(): string {
    return this.details.CompanyDetails.Name;
  }

  public get jobOfferId(): string {
    return this.details.Id;
  }
}
