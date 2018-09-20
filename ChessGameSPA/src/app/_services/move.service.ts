import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Move } from '../_models/move';

@Injectable({
  providedIn: 'root'
})
export class MoveService {
baseUrl = environment.apiUrl + 'move';

constructor(private http: HttpClient) { }

addMove(move: Move) {
  return this.http.post(this.baseUrl, move);
}
}
