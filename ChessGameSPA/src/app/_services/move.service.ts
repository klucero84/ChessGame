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

joinGame(game: Game) {
  // console.log(this.baseUrl + '/ws');
  this.hubConnection = new HubConnectionBuilder().withUrl(this.baseUrl + '/ws').build();
  this.hubConnection
  .start()
  .then(
    () => {
      // console.log('Connection started.');
      this.hubConnection.invoke('joinGame', game.id).then(function (connectionId) {
        // console.log(connectionId);
        game.connId = connectionId;
      });
      this.hubConnection.on('addMoveToGame',  (move: Move) => {
        // console.log(move);
        // get the piece being moved by other player
        const pieceBeingMoved = game.pieces.filter(p => p.x === move.startX && p.y === move.startY)[0];
        // if opponent moved, we are listening the piece will still be here
        // if our piece it's already at it's new location
        if (pieceBeingMoved) {
          const pieceAtEndLocation = game.pieces.filter(p => p.x === move.endX && p.y === move.endY)[0];
          if (pieceAtEndLocation) {
            // determine if the piece already there is our piece
            if (pieceAtEndLocation.ownedBy.id !== move.userId) {
              // if it's not the same user's piece they are capturing
              game.pieces.splice(game.pieces.indexOf(pieceAtEndLocation), 1);
            }
          }
          this.addMoveToGame(move, game);
          pieceBeingMoved.x = move.endX;
          pieceBeingMoved.y = move.endY;
        }
        // console.log(move);
      });
    }
    )
  .catch(
    // err => console.log('Error while establishing connection:' + err)
    );
}

leaveGame(game: Game) {
  this.hubConnection.invoke('leaveGame', game.id);
  this.hubConnection.stop().
  then(
    // () => console.log('Connection ended.')
    )
  .catch(
    // err => console.log('Error while closing connection:' + err)
    );

}

addMoveToGame(move: Move, game: Game) {
  console.log(move);
  if (game.canWhiteQueenSideCastle && move.startY === 0 && move.startX === 0) {
    game.canWhiteQueenSideCastle = false;
  } else if (game.canWhiteKingSideCastle && move.startY === 0 && move.startX === 7) {
    game.canWhiteKingSideCastle = false;
  } else if ((game.canWhiteKingSideCastle || game.canWhiteQueenSideCastle) &&
      move.startY === 0 && move.startX === 4) {
    game.canWhiteKingSideCastle = false;
    game.canWhiteQueenSideCastle = false;
  } else if (game.canBlackQueenSideCastle && move.startY === 0 && move.startX === 0) {
    game.canBlackQueenSideCastle = false;
  } else if (game.canBlackKingSideCastle && move.startY === 0 && move.startX === 7) {
    game.canBlackKingSideCastle = false;
  } else if ((game.canBlackKingSideCastle || game.canBlackQueenSideCastle) &&
      move.startY === 0 && move.startX === 4) {
    game.canBlackKingSideCastle = false;
    game.canBlackQueenSideCastle = false;
  }

  if (move.isCastle) {
      this.applyCastle(move, game);
  }
  return game.moves.push(move);
}

private applyCastle(move: Move, game: Game) {
  // console.log(move);
  let piece: Piece;
  if (move.startX === 4 && move.startY === 0) {
      if (move.endX === 6 && move.endY === 0) {
          piece = game.pieces.filter(p => p.x === 7 && p.y === 0)[0];
          piece.x = 5;
      } else if (move.endX === 2 && move.endY === 0) {
          piece = game.pieces.filter(p => p.x === 0 && p.y === 0)[0];
          piece.x = 3;
      }
  } else if (move.startX === 4 && move.startY === 7) {
      if (move.endX === 6 && move.endY === 7) {
          piece = game.pieces.filter(p => p.x === 7 && p.y === 7)[0];
          piece.x = 5;
      } else if (move.endX === 2 && move.endY === 7) {
          piece = game.pieces.filter(p => p.x === 0 && p.y === 7)[0];
          piece.x = 3;
      }
  }
  // console.log(piece);
}

// listenForNewMove(moves: Move[]) {
// }





}
