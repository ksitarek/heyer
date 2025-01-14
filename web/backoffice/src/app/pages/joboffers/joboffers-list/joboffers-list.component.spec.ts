import { ComponentFixture, TestBed } from '@angular/core/testing';

import { JoboffersListComponent } from './joboffers-list.component';

describe('JoboffersListComponent', () => {
  let component: JoboffersListComponent;
  let fixture: ComponentFixture<JoboffersListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [JoboffersListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(JoboffersListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
