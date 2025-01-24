import { Component, input } from '@angular/core';
import { FormArray, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { NgIcon } from '@ng-icons/core';
import { HlmButtonDirective } from '@spartan-ng/ui-button-helm';
import { HlmInputDirective } from '@spartan-ng/ui-input-helm';
import { HlmLabelDirective } from '@spartan-ng/ui-label-helm';
import { ExperienceLevelControlComponent } from '../experience-level-control/experience-level-control.component';
import { JobOfferForms } from '../joboffer-forms';
import { SkillLevelControlComponent } from '../skill-level-control/skill-level-control.component';

@Component({
  selector: 'h-requirements-form',
  imports: [
    ExperienceLevelControlComponent,
    HlmButtonDirective,
    HlmInputDirective,
    HlmLabelDirective,
    NgIcon,
    ReactiveFormsModule,
    SkillLevelControlComponent,
  ],
  templateUrl: './requirements-form.component.html',
  styleUrl: './requirements-form.component.scss',
})
export class RequirementsFormComponent {
  readonly form = input.required<FormGroup>();

  constructor(private jobOfferForms: JobOfferForms) {}

  public get skills(): FormArray {
    return this.form().get('requirements.skills') as FormArray;
  }

  public addSkill(): void {
    this.skills.push(this.jobOfferForms.skillGroup(null));
  }

  public removeSkill(i: number): void {
    this.skills.controls.splice(i, 1);
  }
}
