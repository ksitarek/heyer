import { Component, inject, input } from '@angular/core';
import { NgIcon } from '@ng-icons/core';
import { HlmButtonDirective } from '@spartan-ng/ui-button-helm';
import { HlmDialogService } from '../../../../../libs/ui/ui-dialog-helm/src/lib/hlm-dialog.service';
import { JobOfferDetails } from '../job-offer-details.model';
import { ApplyDialogComponent } from './apply-dialog/apply-dialog.component';

@Component({
  selector: 'h-apply-button',
  imports: [NgIcon, HlmButtonDirective],
  templateUrl: './apply-button.component.html',
  styleUrl: './apply-button.component.scss',
})
export class ApplyButtonComponent {
  public readonly details = input.required<JobOfferDetails>();
  private readonly dialogService = inject(HlmDialogService);

  public applyClicked(): void {
    this.dialogService.open(ApplyDialogComponent, {
      context: { details: this.details },
    });
  }
}
