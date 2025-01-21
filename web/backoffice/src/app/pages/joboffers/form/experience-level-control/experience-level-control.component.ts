import { Component, forwardRef } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { ExperienceLevel } from '../../joboffer-details';
import { NgFor } from '@angular/common';
import { NgIcon } from '@ng-icons/core';

@Component({
  selector: 'h-experience-level-control',
  imports: [NgFor, NgIcon],
  templateUrl: './experience-level-control.component.html',
  styleUrl: './experience-level-control.component.scss',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: forwardRef(() => ExperienceLevelControlComponent),
    },
  ],
})
export class ExperienceLevelControlComponent implements ControlValueAccessor {
  protected currentValue = ExperienceLevel.Junior;

  protected readonly options = [
    { k: ExperienceLevel.Junior, v: 'Junior' },
    { k: ExperienceLevel.Mid, v: 'Mid' },
    { k: ExperienceLevel.Senior, v: 'Senior' },
    { k: ExperienceLevel.CLevel, v: 'C-Level' },
  ];

  protected isDisabled = false;

  private onChange = (value: string) => {};

  private onTouched = () => {};

  public updateValue(experienceLevel: ExperienceLevel) {
    this.currentValue = experienceLevel;
    this.onChange(this.currentValue);
  }

  public writeValue(obj: any): void {
    this.currentValue = obj;
  }

  public registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  public registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  public setDisabledState?(isDisabled: boolean): void {
    this.isDisabled = isDisabled;
  }

  public getIcon(v: ExperienceLevel) {
    if (v == this.currentValue) {
      return 'lucideCircleCheckBig';
    } else {
      return 'lucideCircle';
    }
  }

  public getIconColor(v: ExperienceLevel) {
    if (v == this.currentValue) {
      return 'text-primary';
    } else {
      return 'text-black';
    }
  }
}
