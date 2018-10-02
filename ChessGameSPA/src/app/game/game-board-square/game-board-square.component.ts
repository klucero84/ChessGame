import { Component, OnInit, Input, Output, EventEmitter, ViewChild, ElementRef } from '@angular/core';
import { Piece } from '../../_models/piece';
import { Game } from '../../_models/game';
import { Move } from '../../_models/move';

@Component({
  selector: 'app-game-board-square',
  templateUrl: './game-board-square.component.html',
  styleUrls: ['./game-board-square.component.css']
})
export class GameBoardSquareComponent implements OnInit {
  @Input()x: number;
  @Input()y: number;
  @Input()game: Game;

  private _peice: Piece;
  @Input()
  get piece(): Piece { return this._peice; }
  set piece(piece: Piece) {
    this._peice = piece;
  }

  @Output() pickUpPiece = new EventEmitter();
  @Output() putDownPiece = new EventEmitter();

  constructor() { }

  ngOnInit() {
  }

  drop() {
    this.putDownPiece.emit({piece: this.piece, x: this.x, y: this.y});
  }

  dragOver() {
    // console.log('here');
  }

  // mouseOver(event) {
  //   console.log(event);
  //   // event.path.x
  //   // Game.getPieceForXY()
  // }

  dragStart() {
    if (this.piece && this.userHasTurn()) {
      this.pickUpPiece.emit(this.piece);
    }
  }

  getCssClass() {
    if (!this.piece || !this.piece.ownedBy) {
      return '';
    }
    let cssString = '';
    if (this.piece.ownedBy.id === this.game.whiteUser.id) {
      cssString = cssString + 'white-' + this.piece.discriminator;
    } else if (this.piece.ownedBy.id === this.game.blackUser.id) {
      cssString = cssString + 'black-' + this.piece.discriminator;
    }
    if (this.userHasTurn()) {
      cssString = cssString + ' active-piece';
    } else {
      cssString = cssString + ' non-active-piece';
    }
    return cssString;
  }

  userHasTurn() {
    // get the last move made
    const move = this.game.moves[this.game.moves.length - 1];
    if (!move) {
      if (this.piece && this.piece.ownedBy) {
        // if no moves it's the white user's turn
        return this.piece.ownedBy.id === this.game.whiteUser.id;
      } else {
        return false;
      }
    }
    // move.user is the last user to make a move
    if (move.user) {
      return move.user.id !== this.piece.ownedBy.id;
    } else {
      return move.userId !== this.piece.ownedBy.id;
    }
  }
}
