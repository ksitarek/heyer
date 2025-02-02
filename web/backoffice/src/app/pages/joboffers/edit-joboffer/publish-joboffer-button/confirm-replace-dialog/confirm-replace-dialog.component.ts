import { Component, inject } from '@angular/core';
import { NgIcon } from '@ng-icons/core';
import { BrnDialogRef } from '@spartan-ng/brain/dialog';
import { HlmButtonDirective } from '@spartan-ng/ui-button-helm';
import { HlmDialogComponent, HlmDialogTitleDirective } from '@spartan-ng/ui-dialog-helm';
import { HlmDialogFooterComponent } from '../../../../../../../libs/ui/ui-dialog-helm/src/lib/hlm-dialog-footer.component';
import { HlmDialogHeaderComponent } from './../../../../../../../libs/ui/ui-dialog-helm/src/lib/hlm-dialog-header.component';

@Component({
  selector: 'h-confirm-replace-dialog',
  imports: [
    NgIcon,
    HlmButtonDirective,
    HlmDialogComponent,
    HlmDialogHeaderComponent,
    HlmDialogTitleDirective,
    HlmDialogFooterComponent,
  ],
  templateUrl: './confirm-replace-dialog.component.html',
  styleUrl: './confirm-replace-dialog.component.scss',
})
export class ConfirmReplaceDialogComponent {
  private readonly _dialogRef = inject<BrnDialogRef>(BrnDialogRef);

  public changeSummary(): void {
    this._dialogRef.close('changeSummary');
  }

  public replace(): void {
    this._dialogRef.close('replace');
  }
}
