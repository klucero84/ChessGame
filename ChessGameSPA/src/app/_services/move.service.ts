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

addMoveTwoPlayer(move: Move) {
  return this.http.post(this.baseUrl + '/two-player', move);
}

isLegalMove(move: Move, isWhite: boolean, isCapturing: boolean) {
  const diffX = Math.abs(move.startX - move.endX);
  const diffY = Math.abs(move.startY - move.startY);
  switch (move.discriminator) {
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
