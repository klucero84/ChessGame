import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { Game } from '../_models/game';
import { Move } from '../_models/move';

@Injectable({
  providedIn: 'root'
})
export class GameService {
  baseUrl = environment.apiUrl + 'game';

constructor(private http: HttpClient) { }

newGame() {
  return this.http.post<Game>(this.baseUrl, null);
}

getGames(): Observable<Game[]> {
  return this.http.get<Game[]>(this.baseUrl + 's');
}

getGame(id): Observable<Game> {
  return this.http.get<Game>(this.baseUrl + '/' + id);
}

getMovesForGame(id): Observable<Move[]> {
  return this.http.get<Move[]>(this.baseUrl + '/' + id + '/moves');
}
}

