import { Component, OnInit, Input } from '@angular/core';
import { Game } from '../../_models/game';
import { Move } from '../../_models/move';

@Component({
  selector: 'app-game-history',
  templateUrl: './game-history.component.html',
  styleUrls: ['./game-history.component.css']
})
export class GameHistoryComponent implements OnInit {
  @Input() game: Game;

  constructor() { }

  ngOnInit() {
  }

  getUserForMove(move: Move) {
    let name = this.game.whiteUser.name;
    // const move = this.game.moves[this.game.moves.length - 1];
    if (!move) {
      return name;
    }
    // console.log(move);
    if (move.userId === this.game.whiteUser.id) {
      name = this.game.whiteUser.name;
    } else if ( move.userId === this.game.blackUser.id) {
      name = this.game.blackUser.name;
    } else if ( move.user.id === this.game.whiteUser.id) {
      name = this.game.whiteUser.name;
    } else if ( move.user.id === this.game.blackUser.id) {
      name = this.game.blackUser.name;
    }
    return name;


    // const name = 'blank';
    // if (move.userId === this.game.whiteUser.id) {
    //   return this.game.whiteUser.name;
    // } else if ( move.userId === this.game.blackUser.id) {
    //   return this.game.blackUser.name;
    // } else if ( move.user.id === this.game.whiteUser.id) {
    //   return this.game.whiteUser.name;
    // } else if ( move.user.id === this.game.blackUser.id) {
    //   return this.game.blackUser.name;
    // }
    // return name;
  }

  getXLocName(x: number) {
    const letters: Array<string> = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h'];
    return letters[x];
  }

  getYLocName(y: number) {
    return y + 1;
  }

}
