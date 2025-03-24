import { inject, Injectable } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';

@Injectable({
  providedIn: 'root',
})
export class ApplyForm {
  private readonly fb: FormBuilder = inject(FormBuilder);

  public readonly form = this.fb.group({
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    resumeKey: ['', Validators.required],
    consentNow: ['', Validators.requiredTrue],
    consentFuture: [''],
  });
}
