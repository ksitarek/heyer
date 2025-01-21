import { NgFor } from '@angular/common';
import { Component, forwardRef } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { NgIcon } from '@ng-icons/core';
import { SkillLevel } from '../../joboffer-details';

@Component({
  selector: 'h-skill-level-control',
  imports: [NgFor, NgIcon],
  templateUrl: './skill-level-control.component.html',
  styleUrl: './skill-level-control.component.scss',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: forwardRef(() => SkillLevelControlComponent),
    },
  ],
})
export class SkillLevelControlComponent implements ControlValueAccessor {
  protected currentValue = SkillLevel.NiceToHave;

  protected readonly options = [
    { k: SkillLevel.NiceToHave, v: 'Nice to have' },
    { k: SkillLevel.Junior, v: 'Junior' },
    { k: SkillLevel.Mid, v: 'Mid' },
    { k: SkillLevel.Senior, v: 'Senior' },
    { k: SkillLevel.Expert, v: 'Expert' },
  ];
  protected isDisabled = false;

  private onChange = (value: string) => {};

  private onTouched = () => {};

  public updateValue(v: SkillLevel) {
    this.currentValue = v;
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

  public getIcon(v: SkillLevel) {
    if (v == this.currentValue) {
      return 'lucideCircleCheckBig';
    } else {
      return 'lucideCircle';
    }
  }

  public getIconColor(v: SkillLevel) {
    if (v == this.currentValue) {
      return 'text-primary';
    } else {
      return 'text-black';
    }
  }
}
