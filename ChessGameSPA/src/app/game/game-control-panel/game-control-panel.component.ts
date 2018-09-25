import { Component, OnInit, Input } from '@angular/core';
import { Game } from '../../_models/game';

@Component({
  selector: 'app-game-control-panel',
  templateUrl: './game-control-panel.component.html',
  styleUrls: ['./game-control-panel.component.css']
})
export class GameControlPanelComponent implements OnInit {
  @Input() game: Game;

  constructor() { }

  ngOnInit() {
  }

  usersTurn() {
    let name = this.game.whiteUser.name;
    const move = this.game.moves[this.game.moves.length - 1];
    if (move.userId === this.game.whiteUser.id) {
      name = this.game.blackUser.name;
    } else if ( move.userId === this.game.blackUser.id) {
      name = this.game.whiteUser.name;
    } else if ( move.user.id === this.game.whiteUser.id) {
      name = this.game.blackUser.name;
    } else if ( move.user.id === this.game.blackUser.id) {
      name = this.game.whiteUser.name;
    }
    return name;
  }

}
