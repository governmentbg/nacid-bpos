import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { NomenclatureSelectModule } from 'src/infrastructure/components/nomenclature-select/nomenclature-select.module';
import { MaterialComponentsModule } from 'src/infrastructure/material-components.module';
import { SharedModule } from 'src/infrastructure/shared.module';
import { LoginComponent } from './components/login.component';
import { UserActivationComponent } from './components/user-activation.component';
import { UserChangePasswordModalComponent } from './components/user-change-password-modal.component';
import { UserCreateComponent } from './components/user-create.component';
import { UserEditComponent } from './components/user-edit.component';
import { UserForgottenPasswordComponent } from './components/user-forgotten-password.component';
import { UserPasswordRecoveryComponent } from './components/user-password-recovery.component';
import { UserSearchComponent } from './components/user-search.component';
import { RoleResource } from './resources/role.resource';
import { UserActivationResource } from './resources/user-activation.resource';
import { UserLoginResource } from './resources/user-login.resource';
import { UserPasswordRecoveryResource } from './resources/user-password-recovery.resource';
import { UserResource } from './resources/user.resource';
import { UserSearchFilterService } from './services/user-search-filter.service';
import { UserRoutingModule } from './user-routing.module';

@NgModule({
  imports: [
    CommonModule,
    SharedModule,
    FormsModule,
    MaterialComponentsModule,
    NomenclatureSelectModule,
    UserRoutingModule,
    TranslateModule
  ],
  declarations: [
    LoginComponent,
    UserForgottenPasswordComponent,
    UserPasswordRecoveryComponent,
    UserSearchComponent,
    UserCreateComponent,
    UserActivationComponent,
    UserEditComponent,
    UserChangePasswordModalComponent
  ],
  entryComponents: [
    UserChangePasswordModalComponent
  ],
  providers: [
    UserSearchFilterService,
    UserLoginResource,
    UserPasswordRecoveryResource,
    UserResource,
    UserActivationResource,
    RoleResource
  ]
})
export class UserModule { }
