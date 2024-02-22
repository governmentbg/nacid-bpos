export class UserSearchResultDto {
  id: number;
  username: string;
  mail: string;
  fullname: string;
  role: string;

  createDate: Date | null;
  updateDate: Date | null;

  isActive: boolean;

  isLocked: boolean;
}
