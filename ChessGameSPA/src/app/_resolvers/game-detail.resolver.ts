import { Injectable } from '@angular/core';
import { Game } from '../_models/game';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { GameService } from '../_services/game.service';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class GameDetailResolver implements Resolve<Game> {

    constructor (private gameService: GameService,
                    private router: Router,
                    private alertify: AlertifyService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<Game> {
        return this.gameService.getGame(route.params['id']).pipe(
            catchError( error => {
                this.alertify.error('Problem retrieving game.');
                this.router.navigate(['/home']);
                return of(null);
            })
        );
    }
}
