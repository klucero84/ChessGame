import { Component, OnInit, Input, HostListener } from '@angular/core';
import { Game } from '../../_models/game';
import { MoveService } from '../../_services/move.service';
import { AlertifyService } from '../../_services/alertify.service';
import { Move } from '../../_models/move';
import { GameStatus } from '../../_models/game-status';

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
    if (!piece || this.game.statusCode >= GameStatus.GAMEOVERMAN) {
      return;
    }
    // if we are already moving a piece stop
    if (this.isMoving) {
      return;
    }
    // this is here because multiple buttons e.g. left, right, middle, etc.
    // cause multiple drag events and we only allow one move at a time.
    this.isMoving = true;

    // initialize move with piece info
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
    this.move.endX = landingSquareInfo.x;
    this.move.endY = landingSquareInfo.y;
    this.move.gameId = this.game.id;
    this.move.connId = this.game.connId;
    this.move.isWhite = this.game.whiteUser.id === this.move.userId;
    const isLegal = Game.isLegalMove(this.game, this.move);
    if (isLegal !== true) {
      this.alertifyService.warning(isLegal.toString());
      this.resetMove();
      return false;
    }
    this.isWaitForServer = true;
    // used in dev to add moves from one user.
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
    if (this.game.statusCode >= GameStatus.GAMEOVERMAN) {
      return;
    }
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
