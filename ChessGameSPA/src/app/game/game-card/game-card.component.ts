import { Component, OnInit } from '@angular/core';
import { Game } from '../../_models/game';

@Component({
  selector: 'app-game-card',
  templateUrl: './game-card.component.html',
  styleUrls: ['./game-card.component.css']
})
export class GameCardComponent implements OnInit {
  game: Game;
  constructor() { }

  ngOnInit() {
  }

  isGameFinished() {
    return this.game.dateCompleted != null;
  }

}
