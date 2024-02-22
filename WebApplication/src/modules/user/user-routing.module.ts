import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from 'src/infrastructure/interceptors/auth.guard';
import { LoginComponent } from './components/login.component';
import { UserActivationComponent } from './components/user-activation.component';
import { UserCreateComponent } from './components/user-create.component';
import { UserEditComponent } from './components/user-edit.component';
import { UserForgottenPasswordComponent } from './components/user-forgotten-password.component';
import { UserPasswordRecoveryComponent } from './components/user-password-recovery.component';
import { UserSearchComponent } from './components/user-search.component';

const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'forgottenPassword',
    component: UserForgottenPasswordComponent
  },
  {
    path: 'passwordRecovery',
    component: UserPasswordRecoveryComponent
  },
  {
    path: 'userActivation',
    component: UserActivationComponent
  },
  {
    path: 'users',
    children: [
      {
        path: '',
        component: UserSearchComponent,
        canActivate: [AuthGuard]
      },
      {
        path: 'new',
        component: UserCreateComponent,
        canActivate: [AuthGuard]
      },
      {
        path: ':id',
        component: UserEditComponent,
        canActivate: [AuthGuard]
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})

export class UserRoutingModule { }
