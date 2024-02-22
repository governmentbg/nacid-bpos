import { HttpErrorResponse } from '@angular/common/http';
import { Component, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { Observable } from 'rxjs';
import { DomainError } from 'src/infrastructure/base/models/domain-error.model';
import { LoadingIndicatorService } from 'src/infrastructure/components/loading-indicator/loading-indicator.service';
import { SystemMessageType } from 'src/infrastructure/system-messages/enums/system-message-type.enum';
import { SystemMessage } from 'src/infrastructure/system-messages/models/system-message.model';
import { SystemMessagesHandlerService } from 'src/infrastructure/system-messages/services/system-messages-handler.service';
import { Institution } from 'src/modules/institution/models/institution.model';
import { UserRoleAliases } from '../constants/user-role-aliases.constant';
import { UserCreationDto } from '../dtos/user-creation.dto';
import { Role } from '../models/role.model';
import { RoleResource } from '../resources/role.resource';
import { UserResource } from '../resources/user.resource';

@Component({
  selector: 'user-create',
  templateUrl: 'user-create.component.html'
})

export class UserCreateComponent {
  @ViewChild('userCreationForm', { static: false }) userCreationForm: NgForm;

  model = new UserCreationDto();

  selectedRole: Role;
  selectedInstitutions: Institution[] = [];

  rolesObservable: Observable<Role[]>;

  userRoleAliases = UserRoleAliases;

  constructor(
    private resource: UserResource,
    private roleResource: RoleResource,
    private router: Router,
    private loadingIndicator: LoadingIndicatorService,
    private systemMessageHandler: SystemMessagesHandlerService,
    public translateService: TranslateService
  ) {
    this.rolesObservable = this.roleResource.getRoles();
  }

  create() {
    if (this.selectedInstitutions && this.selectedInstitutions.length) {
      this.model.institutionIds = this.selectedInstitutions.map(e => e.id);
    }

    this.model.username = this.model.email;
    this.model.roleId = this.selectedRole.id;

    this.loadingIndicator.show();
    this.resource.createUser(this.model)
      .subscribe(
        () => {
          this.loadingIndicator.hide();

          const successMessage = new SystemMessage('app.successMessages.userCreation', SystemMessageType.success, 30);
          this.systemMessageHandler.messages.emit(successMessage);

          this.router.navigate(['users']);
        },
        (err: any) => {
          this.loadingIndicator.hide();

          if (err instanceof HttpErrorResponse) {
            this.userCreationForm.controls['mail'].setErrors(null);
            if (err.status === 422) {
              const data = err.error as DomainError;
              for (let i = 0; i <= data.messages.length - 1; i++) {
                const domainErrorCode = data.messages[i].domainErrorCode;
                if (domainErrorCode === 'User_EmailTaken') {
                  this.userCreationForm.controls['mail'].setErrors({ 'mailTaken': true });
                }
              }
            }
          }
        });
  }

  roleChanged(newRole: Role) {
    this.selectedRole = newRole;
    this.model.orcid = null;
    if (newRole.alias === this.userRoleAliases.ADMINISTRATOR) {
      this.selectedInstitutions = [];
    } else if (newRole.alias === this.userRoleAliases.ORGANIZATION_ADMINISTRATOR || newRole.alias === this.userRoleAliases.MODERATOR) {
      if (this.selectedInstitutions.length) {
        this.selectedInstitutions = [this.selectedInstitutions[0]];
      } else {
        this.selectedInstitutions.push(new Institution());
      }
    } else if (newRole.alias === this.userRoleAliases.SCIENTIST) {
      if (!this.selectedInstitutions || !this.selectedInstitutions.length) {
        this.selectedInstitutions.push(new Institution());
      }
    }
  }

  addInstitution() {
    if (!this.selectedInstitutions) {
      this.selectedInstitutions = [];
    }

    this.selectedInstitutions.push(new Institution());
  }

  removeInstitution(index: number) {
    this.selectedInstitutions.splice(index, 1);
  }

  cancel() {
    this.router.navigate(['users']);
  }
}
