import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Move } from '../_models/move';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { Game } from '../_models/game';

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
        console.log(connectionId);
        game.connId = connectionId;
      });
      this.hubConnection.on('addMoveToGame', (move: Move) => {
        game.moves.push(move);
        const piece = game.pieces.filter(p => p.x === move.startX && p.y === move.startY)[0];
        // if opponent mvoed are listening the piece will still be here
        // if our piece it's at it's new location
        console.log(piece);
        if (piece) {
          piece.x = move.endX;
          piece.y = move.endY;
        } else if (piece.ownedBy.id !== move.userId) {
          // remove the captured piece
          console.log('capturing');
          // game.pieces.splice(game.pieces.indexOf(piece));
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

// listenForNewMove(moves: Move[]) {
// }


isLegalMove(move: Move, isWhite: boolean, isCapturing: boolean) {
  const diffX = Math.abs(move.startX - move.endX);
  const diffY = Math.abs(move.startY - move.startY);
  switch (move.pieceDiscriminator) {
    case 'Pawn':
      if (diffX > 1) { return 'A Pawn cannot move more than one square to the side.'; }
      if (isWhite) {
          // white pawns start at row index 1 and move up one at a time
          if (move.startY + 1 === move.endY) {
              if (diffX === 0) {
                  return true;
              }  else if (isCapturing && diffX === 1) {
                // pawns may move one space to eother side when capturing
                  return true;
              }
          } else if (move.startY + 2 === move.endY && move.startY === 1 && diffX === 0) {
            // if they are at their starting position they may move two spaces
              return true;
          }
      } else {
          // black pawns start at row index 6 and only move down
          if (move.startY - 1 === move.endY ) {
              if (diffX === 0) {
                  return true;
              } else if (isCapturing && diffX === 1) {
                // pawns may move one pace to either side when capturing
                  return true;
              }
          } else if (move.startY - 2 === move.endY && move.startY === 6 && diffX === 0) {
            // if they are at their starting position they may move two spaces
              return true;
          }
      }
      return 'A Pawn may only move forward one space at a time, ' +
      'capture diagonally, and may move two spaces forward if it is the first move of the pawn.';

    case 'Rook':
      if (diffX === 0 || diffY === 0) {
          return true;
      }
      return 'A Rook must move in straight lines along the x or y axis';

    case 'Knight':
      if (diffX === 1 && diffY === 2) {
          return true;
      } else if (diffX === 2 && diffY === 1) {
          return true;
      }
      return 'A Knight must move two spaces on one axis and one space on the other axis.';

    case 'Bishop':
      if (diffX === diffY) {
          return true;
      }
      return 'A Bishop must move in a diagonal line';

    case 'Queen':
      if (diffX === 0 || diffY === 0) {
          return true;
      } else if (diffX === diffY) {
          return true;
      }
      return 'A Queen must move in a in straight lines along the x or y axis, or in a diagonal line.';

    case 'King':
      if (diffX > 1 || diffY > 1) {
          return 'The King can only move one space in any direction.';
      }
      return true;
  }
}


}
