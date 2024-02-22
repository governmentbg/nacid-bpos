
export class UserCreationDto {
  username: string;
  fullname: string;
  email: string;

  orcid: string;

  institutionIds: number[];

  roleId: number;
}
