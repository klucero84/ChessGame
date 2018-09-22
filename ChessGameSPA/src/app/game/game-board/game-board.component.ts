import { Component, OnInit, Input, AfterViewInit } from '@angular/core';
import { Game } from '../../_models/game';
import { MoveService } from '../../_services/move.service';
import { AlertifyService } from '../../_services/alertify.service';
import { Move } from '../../_models/move';

@Component({
  selector: 'app-game-board',
  templateUrl: './game-board.component.html',
  styleUrls: ['./game-board.component.css']
})

export class GameBoardComponent implements OnInit , AfterViewInit {

  boardX: any[];
  boardY: any[];
  move: Move = new Move();
  midMoveX: number;
  midMoveY: number;
  @Input() game: Game;

  constructor(private moveService: MoveService,
              private alertifyService: AlertifyService) {
    this.boardX = Array(0, 1, 2, 3, 4, 5, 6, 7);
    this.boardY = Array(7, 6, 5, 4, 3, 2, 1, 0);

  }

  ngAfterViewInit() {
      this.game.pieces.forEach(piece => {
      this.putPiece(piece);
    });
  }

  ngOnInit() {
  }

  // picking up the peice
  pieceMouseDown(x, y) {
    this.move.startX = x;
    this.move.startY = y;
  }

  // placing the piece
  drop(x, y) {
    const piece = this.getPieceForXY(this.move.startX, this.move.startY);
    const existingPiece = this.getPieceForXY(x, y);
    if (existingPiece !== null) {
      if (this.arePiecesSameTeam(existingPiece, piece)) {
        this.alertifyService.error('Another one of your pieces is already at that location');
      } else {
        // capturing

        this.alertifyService.success('You have captured a piece!');
      }




    }

    this.movePiece(x, y);
  }

  // hovering with the peice
  dragOver(x, y) {
    this.midMoveX = x;
    this.midMoveY = y;
  }

  // pieceMouseUp(x, y) {
  //   console.log('PupX:' + x);
  //   console.log('PupY:' + y);
  // }

  // move current piece to x, y
  movePiece(endX, endY) {
    const piece = this.getPieceForXY(this.move.startX, this.move.startY);
    if (piece === null) { return; }
    if ( this.move.startY !== piece.x || this.move.startY !== piece.y) {
      this.alertifyService.error('piece x/y don\'t match attempted move x/y');
      return;
    }
    this.removePiece(piece);
    piece.x = endX;
    piece.y = endY;
    this.putPiece(piece);
    console.log(piece);
  }

  removePiece(piece) {
    const childDiv = this.getElementForPiece(piece);
    childDiv.classList.remove('piece');
    childDiv.removeAttribute('data-id');
    if (piece.ownedBy.id === this.game.whiteUser.id) {
      childDiv.classList.remove('white-' + piece.discriminator);
    } else if (piece.ownedBy.id === this.game.blackUser.id) {
      childDiv.classList.remove('black-' + piece.discriminator);
    }
  }

  // puts piece at piece's x, y
  putPiece(piece) {
    const childDiv = this.getElementForPiece(piece);
    childDiv.classList.add('piece');
    const idAttr = document.createAttribute('data-id');
    idAttr.value = piece.id.toString();
    childDiv.attributes.setNamedItem(idAttr);
    if (piece.ownedBy.id === this.game.whiteUser.id) {
      childDiv.classList.add('white-' + piece.discriminator);
    } else if (piece.ownedBy.id === this.game.blackUser.id) {
      childDiv.classList.add('black-' + piece.discriminator);
    }
  }

  getElementForPiece(piece) {
    const elements = document.getElementsByClassName('x-' + piece.x + ' y-' + piece.y);
    if (elements.length > 0) {
      return elements[0].firstElementChild;
    }
  }

  getPieceForXY(x, y) {
    const pieces = this.game.pieces.filter(p => p.x === x && p.y === y);
    if (pieces === null) {
      this.alertifyService.error('no piece found at x:' + x + ', y:' + y);
      return;
    } else if (pieces.length > 1) {
      this.alertifyService.error('more than one piece found at x:' + x + ', y:' + y);
      return;
    }
    return pieces[0];
  }

  arePiecesSameTeam(piece1, piece2) {
    return piece1.ownedBy.id === piece2.ownedBy.id;
  }
}
