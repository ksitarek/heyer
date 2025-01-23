import { Component } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { NgIcon } from '@ng-icons/core';
import { SkillLevel } from '../../joboffer-details';

@Component({
  selector: 'h-skill-level-control',
  imports: [NgIcon],
  templateUrl: './skill-level-control.component.html',
  styleUrl: './skill-level-control.component.scss',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: SkillLevelControlComponent,
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

  private onChange: (value: SkillLevel) => void = () => {};

  private onTouched: () => void = () => {};

  public updateValue(v: SkillLevel) {
    this.currentValue = v;
    this.onChange(this.currentValue);
  }

  public writeValue(obj: SkillLevel): void {
    this.currentValue = obj;
  }

  public registerOnChange(fn: (value: SkillLevel) => void): void {
    this.onChange = fn;
  }

  public registerOnTouched(fn: () => void): void {
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

  public getIconClasses(v: SkillLevel) {
    let classes = 'mr-2';

    if (v == this.currentValue) {
      classes += ' text-primary mr-2';
    } else {
      classes += ' text-black mr-2';
    }

    return classes;
  }
}
