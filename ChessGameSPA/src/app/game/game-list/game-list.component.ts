import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Game } from '../../_models/game';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '../../_services/auth.service';
@Component({
  selector: 'app-game-list',
  templateUrl: './game-list.component.html',
  styleUrls: ['./game-list.component.css']
})
export class GameListComponent implements OnInit {

 @Input() games: Game[];

  constructor(private route: ActivatedRoute, private authService: AuthService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.games = data['games'];
    });
  }

  getSide(game: Game) {
    if (this.authService.decodedToken.nameid === game.whiteUser.id.toString()) {
      return 'White';
    } else {
      return 'Black';
    }
  }

  getOpponentName(game: Game) {
    if (this.authService.decodedToken.nameid === game.whiteUser.id.toString()) {
      return game.blackUser.name;
    } else {
      return game.whiteUser.name;
    }
  }

  getSideIcon(game) {
    if (this.authService.decodedToken.nameid === game.whiteUser.id.toString()) {
      return 'fa fa-chess-king white';
    } else {
      return 'fa fa-chess-king black';
    }
  }
}
