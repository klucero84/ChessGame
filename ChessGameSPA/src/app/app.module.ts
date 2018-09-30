import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { BrowserModule } from '@angular/platform-browser';

import { ButtonsModule, BsDropdownModule } from 'ngx-bootstrap';
import { JwtModule } from '@auth0/angular-jwt';

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


// export function tokenGetter() {
//     return localStorage.getItem('token');
// }


@NgModule({
   declarations: [
      AppComponent,
      NavComponent,
      HomeComponent,
      RegisterComponent,
      MessagesComponent,
   ],
   imports: [
      FormsModule,
      BrowserModule,
      HttpClientModule,
      ButtonsModule.forRoot(),
      BsDropdownModule.forRoot(),
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
   ],
   bootstrap: [
      AppComponent
   ],
   exports: [

   ]
})
export class AppModule { }
