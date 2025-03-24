import { Component, computed, input } from '@angular/core';
import { Skill } from '../../../../models/skill.model';

@Component({
  selector: 'h-skill',
  imports: [],
  templateUrl: './skill.component.html',
  styleUrl: './skill.component.scss',
})
export class SkillComponent {
  public readonly skill = input.required<Skill>();

  protected readonly skillLabel = computed(() => {
    return this.skill().Label;
  });

  protected readonly skillLevel = computed(() => {
    return this.skill().Level;
  });
}
