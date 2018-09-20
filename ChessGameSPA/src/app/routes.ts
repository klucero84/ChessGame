import {Routes} from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MessagesComponent } from './messages/messages.component';
import { AuthGuard } from './_guards/auth.guard';
import { PreventUnsavedChanges } from './_guards/prevent-unsaved-changes.guard';
import { UserListComponent } from './user/user-list/user-list.component';
import { UserListResolver } from './_resolvers/user-list.resolver';
import { UserDetailComponent } from './user/user-detail/user-detail.component';
import { UserDetailResolver } from './_resolvers/user-detail.resolver';
import { UserEditComponent } from './user/user-edit/user-edit.component';
import { UserEditResolver } from './_resolvers/user-edit.resolver';
import { GameListComponent } from './game/game-list/game-list.component';
import { GameListResolver } from './_resolvers/game-list.resolver';
import { GamePlayComponent } from './game/game-play/game-play.component';
import { GamePlayResolver } from './_resolvers/game-play.resolver';
import { GameDetailComponent } from './game/game-detail/game-detail.component';
import { GameDetailResolver } from './_resolvers/game-detail.resolver';
import { GameHomeComponent } from './game/game-home/game-home.component';

export const appRoutes: Routes = [
    { path: '', component : HomeComponent },
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children: [
            { path: 'users', component : UserListComponent , resolve: {users: UserListResolver} },
            { path: 'user/:id', component : UserDetailComponent , resolve : {user: UserDetailResolver} },
            { path: 'user/edit', component: UserEditComponent,
            resolve : {user: UserEditResolver}, canDeactivate: [PreventUnsavedChanges] },
            { path: 'messages', component : MessagesComponent },
            { path: 'game',
                children: [
                   { path: 'list', component : GameListComponent, resolve: {games: GameListResolver} },
                   { path: ':id/play', component: GamePlayComponent, resolve : {game: GamePlayResolver} },
                   { path: ':id/detail', component: GameDetailComponent, resolve : {game: GameDetailResolver} },
                   { path: '', component: GameHomeComponent }
                ]
            }
        ]
    },

    { path: '**', redirectTo: '', pathMatch: 'full' }
];
