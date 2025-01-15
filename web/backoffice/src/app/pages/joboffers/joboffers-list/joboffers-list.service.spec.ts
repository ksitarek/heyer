import { TestBed } from '@angular/core/testing';

import { JoboffersListService } from './joboffers-list.service';

describe('JoboffersListService', () => {
  let service: JoboffersListService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(JoboffersListService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
