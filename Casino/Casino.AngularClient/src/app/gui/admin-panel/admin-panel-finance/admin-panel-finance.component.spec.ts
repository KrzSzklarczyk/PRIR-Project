import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminPanelFinanceComponent } from './admin-panel-finance.component';

describe('AdminPanelFinanceComponent', () => {
  let component: AdminPanelFinanceComponent;
  let fixture: ComponentFixture<AdminPanelFinanceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AdminPanelFinanceComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminPanelFinanceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
