import { Component, OnInit, Input } from '@angular/core';
import { Game } from '../../_models/game';
import { GameService } from '../../_services/game.service';
import { Router } from '@angular/router';
import { AlertifyService } from '../../_services/alertify.service';

@Component({
  selector: 'app-game-control-panel',
  templateUrl: './game-control-panel.component.html',
  styleUrls: ['./game-control-panel.component.css']
})
export class GameControlPanelComponent implements OnInit {
  @Input() game: Game;

  constructor(private router: Router,
    private gameService: GameService,
    private alertify: AlertifyService) { }

  ngOnInit() {
  }

  getNewGame() {

    this.gameService.newGame().subscribe(event => {
      // console.log(event);
      this.router.navigate(['/game/' + event.id + '/play']);
    }, error => {
      this.alertify.error(error);
    });
  }

  usersTurn() {
    let name = this.game.whiteUser.name;
    const move = this.game.moves[this.game.moves.length - 1];
    if (!move) {
      return name;
    }
    // console.log(move);
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
