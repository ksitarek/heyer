import { Component, input, output } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { HlmButtonDirective } from '@spartan-ng/ui-button-helm';
import { take, tap } from 'rxjs';
import { NewCandidateApplication } from './new-candidate-application.model';
import { SendApplicationService } from './send-application.service';

@Component({
  selector: 'h-send-application-btn',
  imports: [HlmButtonDirective],
  templateUrl: './send-application-btn.component.html',
  styleUrl: './send-application-btn.component.scss',
})
export class SendApplicationBtnComponent {
  public readonly applyForm = input.required<FormGroup>();
  public readonly jobOfferId = input.required<string>();
  public readonly applicationSent = output();

  constructor(private sendApplicationService: SendApplicationService) {}

  public get disabled() {
    const form = this.applyForm();
    return form.invalid || form.disabled;
  }

  protected submitApplication() {
    if (this.disabled) {
      return;
    }

    this.applyForm().disable();

    const payload = new NewCandidateApplication(
      this.jobOfferId(),
      this.getStringValue('firstName'),
      this.getStringValue('lastName'),
      this.getStringValue('email'),
      this.getStringValue('resumeKey'),
      this.getBooleanValue('consentFuture'),
    );

    this.sendApplicationService
      .newCandidateApply(payload)
      .pipe(
        take(1),
        tap(() => {
          this.applicationSent.emit();
        }),
      )
      .subscribe();
  }

  private getStringValue(key: string): string {
    const form = this.applyForm();

    const control = form.get(key) as FormControl;

    return control.value as string;
  }

  private getBooleanValue(key: string): boolean {
    const form = this.applyForm();

    const control = form.get(key) as FormControl;

    if (control.value === '') {
      return false;
    }

    return control.value as boolean;
  }
}
