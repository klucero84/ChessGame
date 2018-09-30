import { Component, OnInit, Input } from '@angular/core';
import { Game } from '../../_models/game';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-game-detail',
  templateUrl: './game-detail.component.html',
  styleUrls: ['./game-detail.component.css']
})
export class GameDetailComponent implements OnInit {

  @Input() game: Game;

  constructor(private route: ActivatedRoute) { }

  ngOnInit() {
  }

  isGameFinished() {
    return this.game.dateCompleted != null;
  }

}
