import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { BrowserModule } from '@angular/platform-browser';

import { TabsModule, PaginationModule, ButtonsModule, BsDropdownModule } from 'ngx-bootstrap';
import { JwtModule } from '@auth0/angular-jwt';
import { NgxGalleryModule } from 'ngx-gallery';

import { AppRoutes } from './routes';

import { AuthService } from './_services/auth.service';
import { MoveService } from './_services/move.service';
import { GameService } from './_services/game.service';
import { UserService } from './_services/user.service';
import { AlertifyService } from './_services/alertify.service';

import { AuthGuard } from './_guards/auth.guard';
import { PreventUnsavedChanges } from './_guards/prevent-unsaved-changes.guard';

import { ErrorInterceptorProvider } from './_services/error.interceptor';

import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { MessagesComponent } from './messages/messages.component';
import { UserListComponent } from './user/user-list/user-list.component';
import { UserEditComponent } from './user/user-edit/user-edit.component';
import { UserCardComponent } from './user/user-card/user-card.component';
import { UserDetailComponent } from './user/user-detail/user-detail.component';
import { UserListResolver } from './_resolvers/user-list.resolver';
import { UserDetailResolver } from './_resolvers/user-detail.resolver';
import { UserEditResolver } from './_resolvers/user-edit.resolver';
import { UserMatchHistoryComponent } from './user/user-match-history/user-match-history.component';


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
      UserCardComponent,
      UserMatchHistoryComponent
   ],
   imports: [
      FormsModule,
      BrowserModule,
      HttpClientModule,
      NgxGalleryModule,
      TabsModule.forRoot(),
      ButtonsModule.forRoot(),
      BsDropdownModule.forRoot(),
      PaginationModule.forRoot(),
      RouterModule.forRoot(AppRoutes),
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
   ],
   bootstrap: [
      AppComponent
   ],
   exports: [

   ]
})
export class AppModule { }
