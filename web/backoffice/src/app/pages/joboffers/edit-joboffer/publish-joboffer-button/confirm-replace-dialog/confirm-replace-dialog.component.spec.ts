import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfirmReplaceDialogComponent } from './confirm-replace-dialog.component';

describe('ConfirmReplaceDialogComponent', () => {
  let component: ConfirmReplaceDialogComponent;
  let fixture: ComponentFixture<ConfirmReplaceDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ConfirmReplaceDialogComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(ConfirmReplaceDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
