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

export const appRoutes: Routes = [
    { path: '', component : HomeComponent },
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children: [
            { path: 'users', component : UserListComponent , resolve: {users: UserListResolver}},
            { path: 'user/:id', component : UserDetailComponent , resolve : {user: UserDetailResolver}},
            { path: 'user/edit', component: UserEditComponent,
            resolve : {user: UserEditResolver}, canDeactivate: [PreventUnsavedChanges]},
            { path: 'messages', component : MessagesComponent }
        ]
    },

    { path: '**', redirectTo: '', pathMatch: 'full' }
];
