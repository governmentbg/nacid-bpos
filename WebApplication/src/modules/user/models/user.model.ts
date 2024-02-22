import { Entity } from 'src/infrastructure/base/models/entity.model';
import { Role } from './role.model';
import { UserInstitution } from './user-institution.model';

export class User extends Entity {
  username: string;
  fullname: string;
  email: string;

  roleId: number;
  role: Role;

  orcid: string;

  isActive: boolean;
  isLocked: boolean;
  createDate: Date | null;
  updateDate: Date | null;

  institutions: UserInstitution[];
}
