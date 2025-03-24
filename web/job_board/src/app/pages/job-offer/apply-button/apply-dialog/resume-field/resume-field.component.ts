import { Component, effect, ElementRef, OnDestroy, signal, viewChild } from '@angular/core';
import {
  ControlValueAccessor,
  FormBuilder,
  FormControl,
  FormGroup,
  NG_VALUE_ACCESSOR,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { HlmFormFieldComponent } from '@spartan-ng/ui-formfield-helm';
import { HlmInputDirective } from '@spartan-ng/ui-input-helm';
import { HlmLabelDirective } from '@spartan-ng/ui-label-helm';
import { distinct, EMPTY, map, Subject, switchMap, takeUntil } from 'rxjs';
import { ResumeStorageService } from './resume-storage.service';

@Component({
  selector: 'h-resume-field',
  imports: [HlmInputDirective, HlmFormFieldComponent, HlmLabelDirective, ReactiveFormsModule],
  templateUrl: './resume-field.component.html',
  styleUrl: './resume-field.component.scss',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: ResumeFieldComponent,
    },
  ],
})
export class ResumeFieldComponent implements ControlValueAccessor, OnDestroy {
  private destroy$ = new Subject<void>();

  private readonly resumeKey = signal<string | null>(null);

  private readonly disabled = signal<boolean>(false);

  private onChange: (value: string | null) => void = () => {};

  private onTouched: () => void = () => {};

  protected readonly resumeFormGroup: FormGroup;

  protected readonly fileInput = viewChild<ElementRef<HTMLInputElement>>('fileInput');

  constructor(
    private fb: FormBuilder,
    private resumeStorage: ResumeStorageService,
  ) {
    effect(() => {
      if (this.disabled()) {
        this.resumeFormGroup.disable();
      } else {
        this.resumeFormGroup.enable();
      }
    });

    effect(() => {
      const resumeKey = this.resumeKey();
      this.onChange(resumeKey);
    });

    this.resumeFormGroup = this.fb.group({
      resume: new FormControl('', [Validators.required]),
    });

    this.resumeFormGroup.valueChanges
      .pipe(
        takeUntil(this.destroy$),
        distinct(),
        switchMap(() => {
          const files = this.fileInput()?.nativeElement.files ?? [];

          if (this.resumeFormGroup.pristine || this.resumeFormGroup.invalid) {
            return EMPTY;
          }

          if (this.resumeFormGroup.valid && files.length > 0) {
            this.onTouched();

            const file = files[0];
            return this.resumeStorage.store(file).pipe(
              map((storeResult) => {
                return storeResult.fileHandle;
              }),
            );
          }

          return EMPTY;
        }),
        map((resumeKey) => {
          this.resumeKey.set(resumeKey);
        }),
      )
      .subscribe();
  }

  public ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  public writeValue(val: string): void {
    this.resumeKey.set(val);
  }

  public registerOnChange(fn: (value: string | null) => void): void {
    this.onChange = fn;
  }

  public registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  public setDisabledState?(isDisabled: boolean): void {
    this.disabled.set(isDisabled);
  }
}
