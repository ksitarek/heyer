import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SkillLevelControlComponent } from './skill-level-control.component';

describe('SkillLevelControlComponent', () => {
  let component: SkillLevelControlComponent;
  let fixture: ComponentFixture<SkillLevelControlComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SkillLevelControlComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SkillLevelControlComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
