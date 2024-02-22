import { Entity } from './entity.model';

export class Nomenclature extends Entity {
  name: string;
  viewOrder: number | null;
  isActive: boolean;

  // for view purposes only
  isEditMode: boolean;
  originalObject: Nomenclature;
}
