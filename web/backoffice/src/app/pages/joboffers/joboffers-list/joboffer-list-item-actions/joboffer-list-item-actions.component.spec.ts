import { ComponentFixture, TestBed } from '@angular/core/testing';

import { JobofferListItemActionsComponent } from './joboffer-list-item-actions.component';

describe('JobofferListItemActionsComponent', () => {
  let component: JobofferListItemActionsComponent;
  let fixture: ComponentFixture<JobofferListItemActionsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [JobofferListItemActionsComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(JobofferListItemActionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
