import { Component, OnInit, Input } from '@angular/core';
import { Game } from '../../_models/game';
import { MoveService } from '../../_services/move.service';
import { AlertifyService } from '../../_services/alertify.service';
import { Move } from '../../_models/move';

@Component({
  selector: 'app-game-board',
  templateUrl: './game-board.component.html',
  styleUrls: ['./game-board.component.css']
})

export class GameBoardComponent implements OnInit {

  boardX: any[];
  boardY: any[];
  move: Move = new Move();
  @Input() game: Game;

  constructor(private moveService: MoveService,
              private alertifyService: AlertifyService) {
    this.boardX = Array(0, 1, 2, 3, 4, 5, 6, 7);
    this.boardY = Array(7, 6, 5, 4, 3, 2, 1, 0);
  }

  ngOnInit() {
  }

  pickUpPiece(piece) {
    if (!piece) {
      return;
    }
    this.move = new Move();
    this.move.piece = piece;
  }

  putDownPiece(event) {
    if (!this.move.piece) {
      return;
    }
    // if (is not legalmove for this kind of piece)
      // this.alertifyService.error('illegal move for piece type');
      // return;

    // if there is a piece
    if (event.piece) {
      if (event.piece.ownedBy.id === this.move.piece.ownedBy.id) {
        this.alertifyService.warning('You cannot place a piece in a square that another of your pieces sits');
        return;
      } else {
        this.alertifyService.success('Capturing');
        return;
      }
    }

    this.move.piece.x = event.x;
    this.move.piece.y = event.y;
    this.move.endX = event.x;
    this.move.endY = event.x;
    this.move.pieceId = this.move.piece.id;
    this.move.userId = this.move.piece.ownedBy.id;
    this.move.gameId = this.game.id;
    this.move.piece = null;
    // this.moveService.addMove(this.move);
    console.log(this.move);
  }

  getPieceForXY(x, y) {
    const pieces = this.game.pieces.filter(p => p.x === x && p.y === y);
    if (pieces === null) {
      return null;
    } else if (pieces.length > 1) {
      return null;
    }
    return pieces[0];
  }
}
