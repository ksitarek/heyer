import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateJobofferComponent } from './create-joboffer.component';

describe('CreateJobofferComponent', () => {
  let component: CreateJobofferComponent;
  let fixture: ComponentFixture<CreateJobofferComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateJobofferComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(CreateJobofferComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
