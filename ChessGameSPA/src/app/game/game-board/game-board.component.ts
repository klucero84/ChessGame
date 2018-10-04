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
  isMoving = false;
  isWaitForServer = false;
  @Input() game: Game;

  constructor(private moveService: MoveService,
              private alertifyService: AlertifyService) {
    this.boardX = Array(0, 1, 2, 3, 4, 5, 6, 7);
    this.boardY = Array(7, 6, 5, 4, 3, 2, 1, 0);
  }

  ngOnInit() {
    this.moveService.joinGame(this.game, this.addMove);
  }

  pickUpPiece(piece) {
    // no piece to pick up
    if (!piece) {
      return;
    }
    // if we are already moving a piece stop
    if (this.isMoving) {
      return;
    }
    this.isMoving = true;

    // initialize move with pice info
    this.move = new Move();
    this.move.startX = piece.x;
    this.move.startY = piece.y;
    this.move.pieceDiscriminator = piece.discriminator;
    this.move.pieceId = piece.id;
    this.move.userId = piece.ownedBy.id;
  }

  putDownPiece(landingSquareInfo) {
    // if we aren't moving a piece stop
    if (!this.isMoving) {
      this.resetMove();
      return;
    }
    // get the piece from the starting location
    const piece = this.getPieceForXY(this.move.startX, this.move.startY);
    if (!piece) {
      this.resetMove();
      return;
    }
    // determine if a piece is already there.
    if (landingSquareInfo.piece) {
      // determine if the piece already there is our piece
      if (landingSquareInfo.piece.ownedBy.id === this.move.userId) {
        this.alertifyService.warning('Pieces must move to either an unoccupied square or one occupied by an opponent\'s piece.');
        this.resetMove();
        return;
      } else {
        // if it's not the same user's piece they are capturing
        this.move.isCapture = true;
      }
    }

    this.move.endX = landingSquareInfo.x;
    this.move.endY = landingSquareInfo.y;
    this.move.gameId = this.game.id;
    this.move.game = this.game;
    this.move.connId = this.game.connId;
    this.move.isWhite = this.game.whiteUser.id === this.move.userId;
    const isLegal = Game.isLegalMove(this.game, this.move);
    // const isLegal = this.move.isLegalMove(this.move.isCapture);
    if (isLegal !== true) {
      this.alertifyService.warning(isLegal.toString());
      this.resetMove();
      return false;
    }
    this.move.game = null;
    // used in dev to add moves from one user.
    this.isWaitForServer = true;
    this.moveService.addMoveTwoPlayer(this.move).subscribe((newMove: Move) => this.addMove(newMove)
    , error => {
      this.alertifyService.error(error);
    }, () => this.resetMove()
    );
  }

  resetMove() {
    this.isMoving = false;
    this.isWaitForServer = false;
    this.move = null;
  }

  addMove(move: Move) {
    Game.addMoveToGame(move, this.game);
  }

  getPieceForXY(x: number, y: number) {
    // ui hack to move piece image to new location when the user let's go
    // instead of waiting until server ok's the move.
    if (this.isWaitForServer) {
      if (x === this.move.endX && y === this.move.endY) {
        return Game.getPieceForXY(this.game, this.move.startX, this.move.startY);
      } else if (x === this.move.startX && y === this.move.startY) {
        return;
      }
    }
    return Game.getPieceForXY(this.game, x, y);
  }

  @HostListener('window:beforeunload')
  leaveGame() {
    this.moveService.leaveGame(this.game);
  }
}
