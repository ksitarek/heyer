import { TestBed } from '@angular/core/testing';

import { PublishJobofferService } from './publish-joboffer.service';

describe('PublishJobofferService', () => {
  let service: PublishJobofferService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PublishJobofferService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
