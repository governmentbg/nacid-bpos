import { Injectable } from '@angular/core';
import { Configuration } from 'src/infrastructure/base/configuration/configuration';
import { UserRoleAliases } from 'src/modules/user/constants/user-role-aliases.constant';
import { UserRoleService } from 'src/modules/user/services/user-role.service';

@Injectable()
export class UserPermissionVerificatorService {
  constructor(
    private userRoleService: UserRoleService,
    private configuration: Configuration
  ) { }

  hasFullAccess(): boolean {
    return this.userRoleService.hasRole(UserRoleAliases.ADMINISTRATOR);
  }

  isFromPublicationInstitution(institutionId: number): boolean {
    if (!this.userRoleService.hasRole(UserRoleAliases.ORGANIZATION_ADMINISTRATOR) && !this.userRoleService.hasRole(UserRoleAliases.MODERATOR)) {
      return false;
    }

    return this.hasRelatedInstitution(institutionId);
  }

  isOrganizationAdministrator(institutionIds: number[]) {
    if (!this.userRoleService.hasRole(UserRoleAliases.ORGANIZATION_ADMINISTRATOR)) {
      return false;
    }

    let isFromRelatedInstitution = false;
    institutionIds.forEach((institutionId: number) => isFromRelatedInstitution = isFromRelatedInstitution || this.hasRelatedInstitution(institutionId));

    return isFromRelatedInstitution;
  }

  isCreator(publicationUserId: number): boolean {
    if (!this.userRoleService.hasRole(UserRoleAliases.SCIENTIST)) {
      return false;
    }

    const currentUserId = +localStorage.getItem(this.configuration.userIdProperty);
    return publicationUserId == currentUserId;
  }

  private hasRelatedInstitution(institutionId: number): boolean {
    const userInstitutions = localStorage.getItem(this.configuration.userInstitutionsProperty)
      .split(',')
      .map(e => +e);
    return userInstitutions.indexOf(institutionId) >= 0;
  }
}
