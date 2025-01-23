import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ExperienceLevelControlComponent } from './experience-level-control.component';

describe('ExperienceLevelControlComponent', () => {
  let component: ExperienceLevelControlComponent;
  let fixture: ComponentFixture<ExperienceLevelControlComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ExperienceLevelControlComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(ExperienceLevelControlComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
