import { Injectable } from '@angular/core';
import { Configuration } from 'src/infrastructure/base/configuration/configuration';

@Injectable()
export class UserRoleService {
  private userRole: string;

  constructor(private configuration: Configuration) {
    this.userRole = localStorage.getItem(this.configuration.userRolesProperty);
  }

  getRole() {
    if (!this.userRole) {
      this.userRole = localStorage.getItem(this.configuration.userRolesProperty);
    }

    return this.userRole;
  }

  clearRole() {
    this.userRole = null;
  }

  hasRole(role: string) {
    return this.userRole === role;
  }
}
