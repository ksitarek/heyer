import { ExperienceLevel } from './experience-level.model';
import { Skill } from './skill.model';

export class Requirements {
  public constructor(
    public ExperienceLevel: ExperienceLevel,
    public Skills: Skill[],
  ) {}

  public static from(obj: Requirements) {
    return new Requirements(
      obj.ExperienceLevel,
      obj.Skills.map((skill: Skill) => Skill.from(skill)),
    );
  }
}
