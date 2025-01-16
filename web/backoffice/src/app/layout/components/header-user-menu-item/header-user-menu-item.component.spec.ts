import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HeaderUserMenuItemComponent } from './header-user-menu-item.component';

describe('HeaderUserMenuItemComponent', () => {
  let component: HeaderUserMenuItemComponent;
  let fixture: ComponentFixture<HeaderUserMenuItemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HeaderUserMenuItemComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(HeaderUserMenuItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
