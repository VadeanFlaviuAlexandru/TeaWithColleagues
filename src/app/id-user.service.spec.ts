import { TestBed } from '@angular/core/testing';

import { IdUserService } from './id-user.service';

describe('IdUserService', () => {
  let service: IdUserService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(IdUserService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
