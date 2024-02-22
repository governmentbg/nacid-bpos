import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ConfirmDialogComponent } from 'src/infrastructure/components/confirm-modal/confirm-modal.component';
import { LoadingIndicatorService } from 'src/infrastructure/components/loading-indicator/loading-indicator.service';
import { UserPermissionVerificatorService } from 'src/infrastructure/services/user-permission-verification.service';
import { UserRoleAliases } from '../constants/user-role-aliases.constant';
import { UserInstitution } from '../models/user-institution.model';
import { User } from '../models/user.model';
import { UserResource } from '../resources/user.resource';

@Component({
  selector: 'user-edit',
  templateUrl: 'user-edit.component.html'
})
export class UserEditComponent implements OnInit {
  model: User = new User();
  isEditMode = false;
  originalObject: User;

  userRoleAliases = UserRoleAliases;

  isActivationEnabled = false;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private resource: UserResource,
    private userPermissionVerificatorService: UserPermissionVerificatorService,
    private dialog: MatDialog,
    public translateService: TranslateService,
    private loadingIndicator: LoadingIndicatorService
  ) { }

  ngOnInit() {
    this.route.params.subscribe((params: Params) => {
      const id = +params['id'];
      if (id) {
        this.loadingIndicator.show();
        this.resource.getById(id)
          .subscribe((model: User) => {
            this.model = model;

            this.isActivationEnabled = this.userPermissionVerificatorService.hasFullAccess()
              || this.userPermissionVerificatorService.isOrganizationAdministrator(this.model.institutions.map((userInstitution: UserInstitution) => userInstitution.institutionId));

            this.loadingIndicator.hide();
          });
      } else {
        this.router.navigate(['users']);
      }
    });
  }

  save() {
    this.loadingIndicator.show();
    this.resource.update(this.model.id, this.model)
      .subscribe((model: User) => {
        this.model = model;
        this.isEditMode = false;
        this.originalObject = new User();

        this.loadingIndicator.hide();
      });
  }

  edit() {
    if (this.isEditMode) {
      return;
    }

    this.originalObject = JSON.parse(JSON.stringify(this.model));
    this.isEditMode = true;
  }

  cancel() {
    if (!this.isEditMode) {
      return;
    }

    if (!this.model.id) {
      this.router.navigate(['users']);
    }

    this.model = JSON.parse(JSON.stringify(this.originalObject));
    this.originalObject = null;
    this.isEditMode = false;
  }

  addInstitution() {
    if (!this.model.institutions) {
      this.model.institutions = [];
    }

    const newInstitution = new UserInstitution();
    newInstitution.userId = this.model.id;
    this.model.institutions.push(newInstitution);
  }

  removeInstitution(index: number) {
    this.model.institutions.splice(index, 1);
  }

  deactivate() {
    this.translateService.get([
      'user.edit.deactivationConfirmation.header',
      'user.edit.deactivationConfirmation.body',
      'user.edit.deactivationConfirmation.acceptText',
      'user.edit.deactivationConfirmation.declineText'
    ])
      .subscribe((translation: any) => {
        this.dialog.open(ConfirmDialogComponent, {
          disableClose: true,
          data: {
            header: translation['user.edit.deactivationConfirmation.header'],
            body: translation['user.edit.deactivationConfirmation.body'],
            acceptText: translation['user.edit.deactivationConfirmation.acceptText'],
            showDecline: true,
            declineText: translation['user.edit.deactivationConfirmation.declineText']
          }
        })
          .afterClosed()
          .subscribe((result: boolean) => {
            if (result) {
              this.loadingIndicator.show();
              this.resource.deactivate(this.model.id)
                .subscribe((model: User) => {
                  this.model = model;
                  this.loadingIndicator.hide();
                })
            }
          });
      });
  }

  sendActivationMail() {
    this.resource.sendActivationMail(this.model.id)
      .subscribe();
  }
}
