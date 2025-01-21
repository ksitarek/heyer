import { TestBed } from '@angular/core/testing';

import { CreateJobOfferService } from './create-job-offer.service';

describe('CreateJobOfferService', () => {
  let service: CreateJobOfferService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CreateJobOfferService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
