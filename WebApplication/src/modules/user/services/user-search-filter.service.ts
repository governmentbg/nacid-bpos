import { Injectable } from '@angular/core';
import { BaseEntityFilterService } from 'src/infrastructure/base/interfaces/base-entity-filter.service';
import { Role } from '../models/role.model';

@Injectable()
export class UserSearchFilterService extends BaseEntityFilterService {
  username: string;
  name: string;

  roleId: number | null;
  role: Role;

  constructor() {
    super(10);
  }

  clear() {
    this.limit = 10;
    this.offset = 0;

    this.username = null;
    this.name = null;
    this.roleId = null;
    this.role = null;
  }
}
