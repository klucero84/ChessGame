import { Component, OnInit, Input } from '@angular/core';
import { Game } from '../../_models/game';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-game-home',
  templateUrl: './game-home.component.html',
  styleUrls: ['./game-home.component.css']
})
export class GameHomeComponent implements OnInit {

  @Input() games: Game[];
  @Input() game: Game[];

  playMode = false;
  listMode = true;

  constructor(private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.games = data['games'];
      this.game = data['game'];
      // console.log(this.game);
    });
    this.listMode = !this.game;
    this.playMode = !this.games;
  }
}
