/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { MoveService } from './move.service';

describe('Service: Move', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [MoveService]
    });
  });

  it('should ...', inject([MoveService], (service: MoveService) => {
    expect(service).toBeTruthy();
  }));
});
