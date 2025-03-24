import { SkillLevel } from './skill-level.model';

export class Skill {
  public constructor(
    public Label: string,
    public Level: SkillLevel,
  ) {}
  public static from(obj: Skill) {
    return new Skill(obj.Label, obj.Level);
  }
}
