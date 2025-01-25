import { Component, input, OnDestroy, OnInit } from '@angular/core';
import { FormArray, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { NgIcon } from '@ng-icons/core';
import { HlmButtonDirective } from '@spartan-ng/ui-button-helm';
import { HlmInputDirective } from '@spartan-ng/ui-input-helm';
import { HlmLabelDirective } from '@spartan-ng/ui-label-helm';
import { debounceTime, distinct, filter, Subscription, switchMap, tap } from 'rxjs';
import { ExperienceLevel, Skill, SkillLevel } from '../../joboffer-details';
import { JobOfferDetailsService } from '../../joboffers-details.service';
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
export class RequirementsFormComponent implements OnInit, OnDestroy {
  readonly form = input.required<FormGroup>();

  private requirementsSubscription?: Subscription;

  constructor(
    private jobOfferForms: JobOfferForms,
    private jobOfferDetailsService: JobOfferDetailsService,
  ) {}

  public ngOnInit(): void {
    this.requirementsSubscription = this.requirements.valueChanges
      .pipe(
        debounceTime(200),
        distinct(),
        filter(() => this.requirements.dirty),
        filter(() => this.requirements.valid),
        switchMap(
          (requirementsValue: { experienceLevel: ExperienceLevel; skills: { label: string; level: SkillLevel }[] }) =>
            this.jobOfferDetailsService.setRequirements(
              this.jobOfferId,
              requirementsValue.experienceLevel,
              requirementsValue.skills.map((skill) => new Skill(skill.label, skill.level)),
            ),
        ),
        tap(() => {
          this.requirements.markAsPristine();
        }),
      )
      .subscribe();
  }
  public ngOnDestroy(): void {
    this.requirementsSubscription?.unsubscribe();
  }

  public get jobOfferId(): string {
    return this.form().get('id')?.value as string;
  }

  public get requirements(): FormGroup {
    return this.form().get('requirements') as FormGroup;
  }

  public get skills(): FormArray {
    return this.requirements.get('skills') as FormArray;
  }

  public addSkill(): void {
    this.skills.push(this.jobOfferForms.skillGroup(null));
  }

  public removeSkill(i: number): void {
    this.skills.controls.splice(i, 1);
    this.skills.markAsDirty();
    this.skills.updateValueAndValidity();
  }
}
