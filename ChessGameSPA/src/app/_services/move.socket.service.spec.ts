/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { MoveSocketService } from './move.socket.service';

describe('Service: Move.socket.', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [MoveSocketService]
    });
  });

  it('should ...', inject([MoveSocketService], (service: MoveSocketService) => {
    expect(service).toBeTruthy();
  }));
});
