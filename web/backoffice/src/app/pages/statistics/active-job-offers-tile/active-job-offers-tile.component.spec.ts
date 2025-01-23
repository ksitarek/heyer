import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ActiveJobOffersTileComponent } from './active-job-offers-tile.component';

describe('ActiveJobOffersTileComponent', () => {
  let component: ActiveJobOffersTileComponent;
  let fixture: ComponentFixture<ActiveJobOffersTileComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ActiveJobOffersTileComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(ActiveJobOffersTileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
