import { Institution } from 'src/modules/institution/models/institution.model';

export class UserInstitution {
  userId: number;

  institutionId: number;
  institution: Institution;
}
