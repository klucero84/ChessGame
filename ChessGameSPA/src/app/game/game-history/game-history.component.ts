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

  getXLocName(x: number) {
    const letters: Array<string> = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h'];
    return letters[x];
  }

  getYLocName(y: number) {
    return y + 1;
  }

}
