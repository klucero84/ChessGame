import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Move } from '../_models/move';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { Game } from '../_models/game';
import { Piece } from '../_models/piece';

@Injectable({
  providedIn: 'root'
})
export class MoveService {
baseUrl = environment.apiUrl + 'move';
private hubConnection: HubConnection;


constructor(private http: HttpClient) { }


addMove(move: Move) {
  return this.http.post(this.baseUrl, move);
}

addMoveTwoPlayer(move: Move) {
  return this.http.post(this.baseUrl + '/two-player', move);
}

joinGame(game: Game, AddMoveToGameCallback: (move: Move) => void) {
  this.hubConnection = new HubConnectionBuilder().withUrl(this.baseUrl + '/ws').build();
  this.hubConnection
  .start()
  .then(
    () => {
      this.hubConnection.invoke('joinGame', game.id).then(function (connectionId) {
        game.connId = connectionId;
      });
      this.hubConnection.on('addMoveToGame',  (move: Move) => AddMoveToGameCallback(move));
    }
    )
  .catch(
    err => console.log('Error while establishing connection:' + err)
    );
}

leaveGame(game: Game) {
  this.hubConnection.invoke('leaveGame', game.id);
  this.hubConnection.stop().
  then(
    )
  .catch(
    err => console.log('Error while establishing connection:' + err)
    );

}
}
