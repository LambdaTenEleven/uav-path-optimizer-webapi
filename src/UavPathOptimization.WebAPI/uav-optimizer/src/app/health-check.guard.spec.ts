import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { healthCheckGuard } from './health-check.guard';

describe('healthCheckGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) => 
      TestBed.runInInjectionContext(() => healthCheckGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
