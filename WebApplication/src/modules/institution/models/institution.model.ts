import { Entity } from 'src/infrastructure/base/models/entity.model';

export class Institution extends Entity {
  name: string;
  nameEn: string;

  identifier: string;

  repositoryUrl: string;

  areCommonClassificationsVisible: boolean;

  isActive: boolean;

  constructor() {
    super();
    this.isActive = true;
    this.areCommonClassificationsVisible = true;
  }
}
