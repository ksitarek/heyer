import { Component, input } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { NgIcon } from '@ng-icons/core';
import { HlmButtonDirective } from '@spartan-ng/ui-button-helm';
import { of, switchMap, take, tap } from 'rxjs';
import { HlmDialogService } from './../../../../../../libs/ui/ui-dialog-helm/src/lib/hlm-dialog.service';
import { ConfirmReplaceDialogComponent } from './confirm-replace-dialog/confirm-replace-dialog.component';
import { PublishJobofferService } from './publish-joboffer.service';

@Component({
  selector: 'h-publish-joboffer-button',
  imports: [HlmButtonDirective, NgIcon],
  templateUrl: './publish-joboffer-button.component.html',
  styleUrl: './publish-joboffer-button.component.scss',
})
export class PublishJobofferButtonComponent {
  readonly form = input.required<FormGroup>();

  constructor(
    private publishJobofferService: PublishJobofferService,
    private dialogService: HlmDialogService,
    private router: Router,
  ) {}

  public get canPublish(): boolean {
    const contractsDetailsValid = this.form().get('contractsDetails')?.valid ?? false;
    const locationValid = this.form().get('location')?.valid ?? false;
    const requirementsValid = this.form().get('requirements')?.valid ?? false;

    return contractsDetailsValid && locationValid && requirementsValid;
  }

  public publish(): void {
    this.form().disable();

    const jobOfferId = this.form().get('id')?.value as string;

    this.publishJobofferService
      .checkForConflicts(jobOfferId)
      .pipe(
        take(1),
        switchMap((hasConflicts) => {
          if (hasConflicts) {
            const dialogRef = this.dialogService.open(ConfirmReplaceDialogComponent);

            return dialogRef.closed$.pipe(
              switchMap((result) => {
                if (result === 'replace') {
                  return this.publishJobofferService.publish(jobOfferId);
                } else {
                  // edit summary selected
                  return of(false);
                }
              }),
            );
          } else {
            return this.publishJobofferService.publish(jobOfferId);
          }
        }),
        tap((res) => {
          if (res) {
            void this.router.navigate(['/job-offers']);
          } else {
            this.form().enable();
          }
        }),
      )
      .subscribe();
  }
}
