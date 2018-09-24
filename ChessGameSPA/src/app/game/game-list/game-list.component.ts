import { Component, OnInit, Input } from '@angular/core';
import { Game } from '../../_models/game';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-game-list',
  templateUrl: './game-list.component.html',
  styleUrls: ['./game-list.component.css']
})
export class GameListComponent implements OnInit {

 @Input() games: Game[];

  constructor(private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.games = data['games'];
      // console.log(this.games);
    });
  }
}
