import { Injectable } from '@angular/core';
import { Configuration } from '../base/configuration/configuration';

@Injectable()
export class RoleVerificatorService {
  constructor(private configuration: Configuration) {

  }

  hasRole(roleAlias: string): boolean {
    if (!this.configuration.tokenProperty) {
      return false;
    }

    const currentRoleAlias = localStorage.getItem(this.configuration.userRolesProperty);
    return currentRoleAlias === roleAlias;
  }
}
