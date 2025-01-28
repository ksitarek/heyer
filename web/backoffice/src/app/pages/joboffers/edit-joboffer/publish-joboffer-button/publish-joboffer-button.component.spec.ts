import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PublishJobofferButtonComponent } from './publish-joboffer-button.component';

describe('PublishJobofferButtonComponent', () => {
  let component: PublishJobofferButtonComponent;
  let fixture: ComponentFixture<PublishJobofferButtonComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PublishJobofferButtonComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(PublishJobofferButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
