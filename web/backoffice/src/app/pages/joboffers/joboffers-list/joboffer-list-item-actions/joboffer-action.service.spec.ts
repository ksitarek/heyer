import { TestBed } from '@angular/core/testing';

import { JobofferActionService } from './joboffer-action.service';

describe('JobofferActionService', () => {
  let service: JobofferActionService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(JobofferActionService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
