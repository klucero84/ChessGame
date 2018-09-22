import { Component, OnInit } from '@angular/core';
import { Piece } from '../../_models/piece';

@Component({
  selector: 'app-game-piece',
  templateUrl: './game-piece.component.html',
  styleUrls: ['./game-piece.component.css']
})
export class GamePieceComponent implements OnInit {
  piece: Piece;
  constructor() { }

  ngOnInit() {
    // if (elements.length > 0) {
      // elements[0].firstElementChild.classList.add(type);
      // elements[0].firstElementChild.classList.add('piece');
      // if (piece.ownedBy.id === this.game.whiteUser.id) {
      //   elements[0].firstElementChild.classList.add('white-piece');
      // } else if (piece.ownedBy.id === this.game.blackUser.id) {
      //   elements[0].firstElementChild.classList.add('black-piece');
      // }
    // }
  }

  dragEnd(event) {
    console.log(event);
  }

  pieceMouseDown() {
    console.log('downX:' + this.piece.x);
    console.log('downY:' + this.piece.y);
    // console.log(event);
    // const classes = event.srcElement.classList;
    // console.log(classes[1]);
  }
}
