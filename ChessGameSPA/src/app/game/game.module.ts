import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';

import { GameHomeComponent } from './game-home/game-home.component';
import { GameListComponent } from './game-list/game-list.component';
import { GameBoardComponent } from './game-board/game-board.component';
import { GameDetailComponent } from './game-detail/game-detail.component';
import { GameHistoryComponent } from './game-history/game-history.component';
import { GameBoardSquareComponent } from './game-board-square/game-board-square.component';
import { GameControlPanelComponent } from './game-control-panel/game-control-panel.component';

import { GamePlayResolver } from '../_resolvers/game-play.resolver';
import { GameListResolver } from '../_resolvers/game-list.resolver';
import { GameDetailResolver } from '../_resolvers/game-detail.resolver';
import { DataTableModule } from 'angular-6-datatable';
import { DragAndDropModule } from 'angular-draggable-droppable';

const routes: Routes = [
      { path: 'play/:id', component: GameHomeComponent, resolve : {game: GamePlayResolver} },
      { path: 'detail/:id', component: GameHomeComponent, resolve : {games: GameListResolver} },
      { path: '**', redirectTo: '', component: GameHomeComponent, resolve: {games: GameListResolver} }
];

@NgModule({
  imports: [
    CommonModule,
    DataTableModule,
    DragAndDropModule,
    RouterModule.forChild(routes)
  ],
  declarations: [
    GameHomeComponent,
    GameListComponent,
    GameBoardComponent,
    GameDetailComponent,
    GameHistoryComponent,
    GameBoardSquareComponent,
    GameControlPanelComponent],
  exports: [

  ],
  providers: [
    GamePlayResolver,
    GameListResolver,
    GameDetailResolver,
  ]
})
export class GameModule { }
