import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RemoteWorkControlComponent } from './remote-work-control.component';

describe('RemoteWorkControlComponent', () => {
  let component: RemoteWorkControlComponent;
  let fixture: ComponentFixture<RemoteWorkControlComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RemoteWorkControlComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(RemoteWorkControlComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
