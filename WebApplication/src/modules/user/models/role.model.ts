import { Entity } from 'src/infrastructure/base/models/entity.model';

export class Role extends Entity {
  name: string;
  alias: string;
}
