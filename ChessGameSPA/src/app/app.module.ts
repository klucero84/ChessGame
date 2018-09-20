import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { MessagesComponent } from './messages/messages.component';
import { UserDetailComponent } from './user/user-detail/user-detail.component';
import { UserEditComponent } from './user/user-edit/user-edit.component';
import { UserListComponent } from './user/user-list/user-list.component';
import { AuthGuard } from './_guards/auth.guard';
import { AuthService } from './_services/auth.service';
import { PreventUnsavedChanges } from './_guards/prevent-unsaved-changes.guard';
import { AlertifyService } from './_services/alertify.service';
import { UserService } from './_services/user.service';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { appRoutes } from './routes';
import { HttpClientModule } from '@angular/common/http';
import { ErrorInterceptorProvider } from './_services/error.interceptor';
import { GameService } from './_services/game.service';
import { GameListComponent } from './game/game-list/game-list.component';
import { GameDetailComponent } from './game/game-detail/game-detail.component';
import { GameHomeComponent } from './game/game-home/game-home.component';
import { GamePlayComponent } from './game/game-play/game-play.component';
import { MoveService } from './_services/move.service';
import { GameDetailResolver } from './_resolvers/game-detail.resolver';
import { GameListResolver } from './_resolvers/game-list.resolver';
import { GamePlayResolver } from './_resolvers/game-play.resolver';
import { JwtModule } from '@auth0/angular-jwt';
import { UserListResolver } from './_resolvers/user-list.resolver';
import { UserDetailResolver } from './_resolvers/user-detail.resolver';
import { UserEditResolver } from './_resolvers/user-edit.resolver';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';


export function tokenGetter() {
    return localStorage.getItem('token');
}


@NgModule({
   declarations: [
      AppComponent,
      NavComponent,
      HomeComponent,
      RegisterComponent,
      MessagesComponent,
      UserDetailComponent,
      UserEditComponent,
      UserListComponent,
      GameListComponent,
      GameDetailComponent,
      GameHomeComponent,
      GamePlayComponent
   ],
   imports: [
      BrowserModule,
      HttpClientModule,
      FormsModule,
      BsDropdownModule.forRoot(),
      RouterModule.forRoot(appRoutes),
      JwtModule.forRoot({
        config: {
            tokenGetter: AuthService.prototype.getAuthToken,
            whitelistedDomains: ['localhost:5000'],
            blacklistedRoutes: ['localhost:5000/api/auth']
        }
    })
   ],
   providers: [
       AuthGuard,
       AuthService,
       PreventUnsavedChanges,
       AlertifyService,
       ErrorInterceptorProvider,
       UserService,
       GameService,
       MoveService,
       UserListResolver,
       UserDetailResolver,
       UserEditResolver,
       GameDetailResolver,
       GameListResolver,
       GamePlayResolver
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
