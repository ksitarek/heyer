import { NgFor } from '@angular/common';
import { Component } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { NgIcon } from '@ng-icons/core';
import { ExperienceLevel } from '../../joboffer-details';

@Component({
  selector: 'h-experience-level-control',
  imports: [NgFor, NgIcon],
  templateUrl: './experience-level-control.component.html',
  styleUrl: './experience-level-control.component.scss',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: ExperienceLevelControlComponent,
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

  private onChange: (value: ExperienceLevel) => void = () => {};

  private onTouched: () => void = () => {};

  public updateValue(experienceLevel: ExperienceLevel) {
    this.currentValue = experienceLevel;
    this.onChange(this.currentValue);
  }

  public writeValue(obj: ExperienceLevel): void {
    this.currentValue = obj;
  }

  public registerOnChange(fn: (value: ExperienceLevel) => void): void {
    this.onChange = fn;
  }

  public registerOnTouched(fn: () => void): void {
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

  public getIconClasses(v: ExperienceLevel) {
    let classes = 'mr-2';

    if (v == this.currentValue) {
      classes += ' text-primary mr-2';
    } else {
      classes += ' text-black mr-2';
    }

    return classes;
  }
}
