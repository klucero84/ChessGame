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
    if (this.game.moves && this.game.moves[this.game.moves.length - 1]) {
      return this.game.moves[this.game.moves.length - 1].userId === this.game.whiteUser.id ?
                                          this.game.blackUser.name : this.game.whiteUser.name;
    }
    return this.game.whiteUser.name;
  }

}
