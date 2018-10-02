import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { Game } from '../../_models/game';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-game-home',
  templateUrl: './game-home.component.html',
  styleUrls: ['./game-home.component.css']
})
export class GameHomeComponent implements OnInit {

  @Input() games: Game[];
  @Input() game: Game;
  gameDetail: Game;

  playMode = false;
  listMode = true;
  detailMode = false;

  constructor(private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.games = data['games'];
      this.game = data['game'];
      if (this.game) {
        Game.getPossibleMovesforAllPieces(this.game);

      }
    });
    this.route.url.subscribe(data => {
      if (data[0]) {
      const mode = data[0]['path'];
        if (mode === 'play') {
          this.listMode = false;
          this.playMode = true;
          this.detailMode = false;
          this.gameDetail = null;
        } else if (mode === 'detail') {
          this.gameDetail = this.games.filter(g => g.id === +data[1]['path'])[0];
          this.listMode = true;
          this.playMode = false;
          this.detailMode = true;
        } else {
          this.listMode = true;
          this.playMode = false;
          this.detailMode = false;
          this.gameDetail = null;
        }
      }
    });
  }
}
