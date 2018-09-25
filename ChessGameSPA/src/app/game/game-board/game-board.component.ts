import { Component, OnInit, Input, HostListener } from '@angular/core';
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
  ioConnection: any;




  @Input() game: Game;

  constructor(private moveService: MoveService,
              private alertifyService: AlertifyService) {
    this.boardX = Array(0, 1, 2, 3, 4, 5, 6, 7);
    this.boardY = Array(7, 6, 5, 4, 3, 2, 1, 0);
  }

  ngOnInit() {
    // console.log(this.game.id);
    this.moveService.joinGame(this.game);
  }

  pickUpPiece(piece) {
    if (!piece) {
      return;
    }
    this.move = new Move();
    this.move.startX = piece.x;
    this.move.startY = piece.y;
    this.move.pieceDiscriminator = piece.discriminator;
    this.move.pieceId = piece.id;
    this.move.userId = piece.ownedBy.id;
  }

  showMoves() {
    // console.log(this.game.moves);
  }

  putDownPiece(event) {
    if (event.piece) {
      if (event.piece.ownedBy.id === this.move.userId) {
        this.alertifyService.warning('You cannot place a piece in a square that another of your pieces sits');
        return;
      } else {
        this.alertifyService.success('Capturing');
        return;
      }
    }

      // if there is a piece
    const piece = this.getPieceForXY(this.move.startX, this.move.startY);
    if (!piece) {
      return;
    }
    // if (is not legalmove for this kind of piece)
      // this.alertifyService.error('illegal move for piece type');
      // return;

    // move the piece right now and move it back if it fails to persist
    piece.x = event.x;
    piece.y = event.y;
    this.move.endX = event.x;
    this.move.endY = event.y;
    this.move.gameId = this.game.id;
    this.move.isWhite = this.game.whiteUser.id === this.move.userId;
    if (!this.moveService.isLegalMove(this.move, this.move.isWhite, false)) {
      return false;
    }
    // used in dev to add moves from one user.
    this.moveService.addMoveTwoPlayer(this.move).subscribe(() => {
    // this.moveService.addMove(this.move).subscribe(() => {
      // console.log(this.move);
      // this.game.moves.push(this.move);
      // const savedPiece = this.getPieceForXY(this.move.endX, this.move.endY);
      piece.x = this.move.endX;
      piece.y = this.move.endY;
      // this.alertifyService.success('registration successful');
    }, error => {
      piece.x = this.move.startX;
      piece.y = this.move.startY;
      this.alertifyService.error(error);
    });


  }

  getPieceForXY(x: number, y: number) {
    const pieces = this.game.pieces.filter(p => p.x === x && p.y === y);
    if (pieces === null) {
      return null;
    } else if (pieces.length > 1) {
      return null;
    }
    return pieces[0];
  }

  @HostListener('window:beforeunload')
  leaveGame() {
    this.moveService.leaveGame(this.game.id);
  }
}
