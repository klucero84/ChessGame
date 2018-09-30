import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserListResolver } from '../_resolvers/user-list.resolver';
import { UserDetailResolver } from '../_resolvers/user-detail.resolver';
import { UserEditResolver } from '../_resolvers/user-edit.resolver';
import { UserDetailComponent } from './user-detail/user-detail.component';
import { UserEditComponent } from './user-edit/user-edit.component';
import { UserListComponent } from './user-list/user-list.component';
import { UserCardComponent } from './user-card/user-card.component';
import { UserMatchHistoryComponent } from './user-match-history/user-match-history.component';
import { Routes, RouterModule } from '@angular/router';
import { PreventUnsavedChanges } from '../_guards/prevent-unsaved-changes.guard';
import { TabsModule, PaginationModule } from 'ngx-bootstrap';
import { FormsModule } from '@angular/forms';
import { NgxGalleryModule } from 'ngx-gallery';

const routes: Routes = [
  { path: 'edit', component: UserEditComponent, resolve : {user: UserEditResolver}, canDeactivate: [PreventUnsavedChanges] },
  { path: ':id', component : UserDetailComponent , resolve : {user: UserDetailResolver} },
  { path: '**', component : UserListComponent , resolve: {users: UserListResolver} }
];



@NgModule({
  imports: [
    FormsModule,
    CommonModule,
    NgxGalleryModule,
    TabsModule.forRoot(),
    PaginationModule.forRoot(),
    RouterModule.forChild(routes)
  ],
  declarations: [
    UserDetailComponent,
    UserEditComponent,
    UserListComponent,
    UserCardComponent,
    UserMatchHistoryComponent
  ],
  providers: [
    UserListResolver,
    UserDetailResolver,
    UserEditResolver
  ]
})
export class UserModule { }
