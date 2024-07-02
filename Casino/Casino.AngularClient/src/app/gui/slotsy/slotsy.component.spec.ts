import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SlotsyComponent } from './slotsy.component';

describe('SlotsyComponent', () => {
  let component: SlotsyComponent;
  let fixture: ComponentFixture<SlotsyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [SlotsyComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SlotsyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
