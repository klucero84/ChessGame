import { Component, OnInit, Input, HostListener } from '@angular/core';
import { Game } from '../../_models/game';
import { MoveService } from '../../_services/move.service';
import { AlertifyService } from '../../_services/alertify.service';
import { Move } from '../../_models/move';
import { Piece } from '../../_models/piece';

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
  isMoving = false;




  @Input() game: Game;

  constructor(private moveService: MoveService,
              private alertifyService: AlertifyService) {
    this.boardX = Array(0, 1, 2, 3, 4, 5, 6, 7);
    this.boardY = Array(7, 6, 5, 4, 3, 2, 1, 0);
  }

  ngOnInit() {
    // console.log(this.game);
    this.moveService.joinGame(this.game);
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
    let isCapturing = false;
    // determine if a piece is already there.
    if (landingSquareInfo.piece) {
      // determine if the piece already there is our piece
      if (landingSquareInfo.piece.ownedBy.id === this.move.userId) {
        this.alertifyService.warning('Pieces must move to either an unoccupied square or one occupied by an opponent\'s piece.');
        this.resetMove();
        return;
      } else {
        // if it's not the same user's piece they are capturing
        // this.alertifyService.success('Capturing');
        isCapturing = true;
        // this.resetMove();
        // return;
      }
    }

    this.move.endX = landingSquareInfo.x;
    this.move.endY = landingSquareInfo.y;
    this.move.gameId = this.game.id;
    this.move.game = this.game;
    this.move.connId = this.game.connId;
    this.move.isWhite = this.game.whiteUser.id === this.move.userId;
    const isLegal = this.move.isLegalMove(isCapturing);
    if (isLegal !== true) {
      this.alertifyService.warning(isLegal.toString());
      this.resetMove();
      return false;
    }

    // move the piece right now and move it back if it fails to persist
    piece.x = landingSquareInfo.x;
    piece.y = landingSquareInfo.y;
    this.move.game = null;
    // console.log(this.move);
    // used in dev to add moves from one user.
    this.moveService.addMoveTwoPlayer(this.move).subscribe(() => {
    // this.moveService.addMove(this.move).subscribe(() => {
      // -------------------------
      // Subscribe stuff only executes when we make a move in our screen
      // our opponents moves are handled by the subscription to the move service
      const pieces = this.game.pieces.filter(p => p.x === this.move.endX && p.y === this.move.endY);
      // console.log(pieces);
      if (pieces.length === 2 && isCapturing) {
        // if both players have a piece there
        if ((pieces[0].ownedBy.id === this.game.whiteUser.id ||
            pieces[1].ownedBy.id === this.game.whiteUser.id)
            && (pieces[0].ownedBy.id === this.game.blackUser.id ||
                pieces[1].ownedBy.id === this.game.blackUser.id)) {
            // get the one that doesn't belong to us
            const opponentPiece = pieces.filter(p => p.ownedBy.id !== this.move.userId)[0];
            this.game.pieces.splice(this.game.pieces.indexOf(opponentPiece), 1);
        }
      }
      this.moveService.addMoveToGame(this.move, this.game);
      // this.game.addMoveToGame(this.move);
    }, error => {
      piece.x = this.move.startX;
      piece.y = this.move.startY;
      this.alertifyService.error(error);
    }, () => {
      this.resetMove();
    });
  }

  resetMove() {
    this.isMoving = false;
    this.move = null;
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

  private applyCastle(move: Move) {
    let piece: Piece;
    if (move.startX === 4 && move.startY === 0) {
        if (move.endX === 6 && move.endY === 0) {
            piece = this.game.pieces.filter(p => p.x === 7 && p.y === 0)[0];
            piece.x = 5;
        } else if (move.endX === 2 && move.endY === 0) {
            piece = this.game.pieces.filter(p => p.x === 0 && p.y === 0)[0];
            piece.x = 3;
        }
    } else if (move.startX === 4 && move.startY === 7) {
        if (move.endX === 6 && move.endY === 7) {
            piece = this. game.pieces.filter(p => p.x === 7 && p.y === 0)[0];
            piece.x = 5;
        } else if (move.endX === 2 && move.endY === 7) {
            piece = this. game.pieces.filter(p => p.x === 0 && p.y === 7)[0];
            piece.x = 3;
        }
    }
}

  @HostListener('window:beforeunload')
  leaveGame() {
    this.moveService.leaveGame(this.game);
  }
}
