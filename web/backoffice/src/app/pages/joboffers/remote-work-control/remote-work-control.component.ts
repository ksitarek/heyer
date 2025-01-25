import { Component } from '@angular/core';
import { ControlValueAccessor, FormsModule, NG_VALUE_ACCESSOR, ReactiveFormsModule } from '@angular/forms';
import { NgIcon } from '@ng-icons/core';
import { BrnRadioComponent } from '@spartan-ng/brain/radio-group';
import { HlmRadioGroupComponent } from '@spartan-ng/ui-radiogroup-helm';
import { RemoteWork } from './remote-work';

@Component({
  selector: 'h-remote-work-control',
  imports: [NgIcon, FormsModule, ReactiveFormsModule, BrnRadioComponent, HlmRadioGroupComponent],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: RemoteWorkControlComponent,
    },
  ],
  templateUrl: './remote-work-control.component.html',
  styleUrl: './remote-work-control.component.scss',
})
export class RemoteWorkControlComponent implements ControlValueAccessor {
  protected remoteWork = RemoteWork.Hybrid;

  private static readonly commonRadioClasses =
    'flex flex-col items-center justify-between p-4 border-2 rounded-md w-52 bg-popover';
  private static readonly enabledRadioClasses =
    'border-muted cursor-pointer hover:bg-secondary hover:text-secondary-foreground group-data-[checked=true]:border-primary';
  private static readonly disabledRadioClasses =
    'border-muted cursor-not-allowed group-data-[checked=true]:border-primary opacity-50';

  protected isDisabled = false;

  private onChange: (value: string) => void = () => {};

  private onTouched: () => void = () => {};

  public updateValue($event: RemoteWork) {
    this.remoteWork = $event;
    this.onChange(this.remoteWork);
  }

  public writeValue(value: RemoteWork): void {
    this.remoteWork = value;
  }

  public registerOnChange(fn: (value: string) => void): void {
    this.onChange = fn;
  }

  public registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  public setDisabledState?(isDisabled: boolean): void {
    this.isDisabled = isDisabled;
  }

  public get radioClasses(): string {
    const stateClasses = this.isDisabled
      ? RemoteWorkControlComponent.disabledRadioClasses
      : RemoteWorkControlComponent.enabledRadioClasses;

    return `${RemoteWorkControlComponent.commonRadioClasses} ${stateClasses}`;
  }
}
