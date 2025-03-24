import { Component, computed, input } from '@angular/core';
import { ExperienceLevel } from '../../../models/experience-level.model';
import { Requirements } from '../../../models/requirements.model';
import { SkillComponent } from './skill/skill.component';

@Component({
  selector: 'h-requirements',
  imports: [SkillComponent],
  templateUrl: './requirements.component.html',
  styleUrl: './requirements.component.scss',
})
export class RequirementsComponent {
  public readonly requirements = input.required<Requirements>();

  protected readonly experienceLevelLabel = computed(() => {
    const exp = this.requirements().ExperienceLevel;

    switch (exp) {
      case ExperienceLevel.Junior:
        return 'Junior';
      case ExperienceLevel.Mid:
        return 'Mid';
      case ExperienceLevel.Senior:
        return 'Senior';
      case ExperienceLevel.CLevel:
        return 'C-Level';

      default:
        return 'Unknown Level';
    }
  });

  protected readonly skills = computed(() => {
    // todo sort skills from most important to least important
    return this.requirements().Skills;
  });
}
