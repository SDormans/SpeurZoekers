import { TestBed } from '@angular/core/testing';

import { CollectibleTypeService } from './collectible-type.service';

describe('CollectibleTypeService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: CollectibleTypeService = TestBed.get(CollectibleTypeService);
    expect(service).toBeTruthy();
  });
});
