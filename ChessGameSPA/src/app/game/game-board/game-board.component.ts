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
    if (!this.isMoving) {
      this.isMoving = true;
    } else {
      return false;
    }

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

  putDownPiece(landingSquareInfo) {
    if (!this.isMoving) {
      return;
    }
    // get the piece from the starting location
    const piece = this.getPieceForXY(this.move.startX, this.move.startY);
    if (!piece) {
      return;
    }
    let isCapturing = false;
    if (landingSquareInfo.piece) {
      if (landingSquareInfo.piece.ownedBy.id === this.move.userId) {
        this.alertifyService.warning('You cannot place a piece in a square that another of your pieces sits');
        return;
      } else {
        // this.alertifyService.success('Capturing');
        isCapturing = true;
        return;
      }
    }
    // move the piece right now and move it back if it fails to persist
    piece.x = landingSquareInfo.x;
    piece.y = landingSquareInfo.y;

    this.move.endX = landingSquareInfo.x;
    this.move.endY = landingSquareInfo.y;
    this.move.gameId = this.game.id;
    this.move.connId = this.game.connId;
    this.move.isWhite = this.game.whiteUser.id === this.move.userId;
    const isLegal = this.moveService.isLegalMove(this.move, this.move.isWhite, isCapturing);
    if (!isLegal) {
      this.alertifyService.warning(isLegal.toString());
      return false;
    }

    // used in dev to add moves from one user.
    this.moveService.addMoveTwoPlayer(this.move).subscribe(() => {
    // this.moveService.addMove(this.move).subscribe(() => {
      // console.log(this.move);
      // this.game.moves.push(this.move);
      // const savedPiece = this.getPieceForXY(this.move.endX, this.move.endY);
      // savedPiece.x = this.move.endX;
      // savedPiece.y = this.move.endY;
      // this.alertifyService.success('registration successful');
    }, error => {
      piece.x = this.move.startX;
      piece.y = this.move.startY;
      this.alertifyService.error(error);
    }, () => {
      this.isMoving = false;
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
    this.moveService.leaveGame(this.game);
  }
}
