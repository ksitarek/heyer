import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ContractsDetailsFormComponent } from './contracts-details-form.component';

describe('ContractsDetailsFormComponent', () => {
  let component: ContractsDetailsFormComponent;
  let fixture: ComponentFixture<ContractsDetailsFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ContractsDetailsFormComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ContractsDetailsFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
