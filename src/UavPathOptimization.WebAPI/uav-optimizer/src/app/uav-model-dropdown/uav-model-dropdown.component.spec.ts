import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UavModelDropdownComponent } from './uav-model-dropdown.component';

describe('UavModelDropdownComponent', () => {
  let component: UavModelDropdownComponent;
  let fixture: ComponentFixture<UavModelDropdownComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [UavModelDropdownComponent]
    });
    fixture = TestBed.createComponent(UavModelDropdownComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
