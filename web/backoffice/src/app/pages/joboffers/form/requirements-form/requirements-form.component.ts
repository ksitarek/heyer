import { Component, Input } from '@angular/core';
import {
  FormArray,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
} from '@angular/forms';
import { ExperienceLevelControlComponent } from '../experience-level-control/experience-level-control.component';
import { NgFor } from '@angular/common';
import { HlmInputDirective } from '@spartan-ng/ui-input-helm';
import { SkillLevelControlComponent } from '../skill-level-control/skill-level-control.component';
import { NgIcon } from '@ng-icons/core';
import { HlmButtonDirective } from '@spartan-ng/ui-button-helm';
import { JobOfferForms } from '../joboffer-forms';

@Component({
  selector: 'h-requirements-form',
  imports: [
    NgFor,
    ReactiveFormsModule,
    ExperienceLevelControlComponent,
    HlmInputDirective,
    SkillLevelControlComponent,
    NgIcon,
    HlmButtonDirective,
  ],
  templateUrl: './requirements-form.component.html',
  styleUrl: './requirements-form.component.scss',
})
export class RequirementsFormComponent {
  @Input({ required: true }) form!: FormGroup;

  constructor(private jobOfferForms: JobOfferForms) {}

  public get skills(): FormArray {
    return this.form.get('requirements.skills') as FormArray;
  }

  public addSkill(): void {
    this.skills.push(this.jobOfferForms.skillGroup(null));
  }

  public removeSkill(i: number): void {
    this.skills.controls.splice(i, 1);
  }
}
