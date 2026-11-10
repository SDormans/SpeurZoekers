import { TestBed } from '@angular/core/testing';

import { UserCollectibleService } from './user-collectible.service';

describe('UserCollectibleService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: UserCollectibleService = TestBed.get(UserCollectibleService);
    expect(service).toBeTruthy();
  });
});
