import { TestBed } from '@angular/core/testing';

import { SlotsyService } from './slotsy.service';

describe('SlotsyService', () => {
  let service: SlotsyService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SlotsyService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
