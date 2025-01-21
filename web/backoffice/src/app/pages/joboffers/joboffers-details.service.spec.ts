import { TestBed } from '@angular/core/testing';

import { JobOfferDetailsService } from './joboffers-details.service';

describe('JoboffersDetailsService', () => {
  let service: JobOfferDetailsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(JobOfferDetailsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
