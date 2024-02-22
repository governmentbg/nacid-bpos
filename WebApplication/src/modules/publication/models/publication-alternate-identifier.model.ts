import { ResourceIdentifierType } from 'src/modules/nomenclature/models/resource-identifier-type.model';
import { PublicationEntity } from './publication-entity.model';

export class PublicationAlternateIdentifier extends PublicationEntity {
  value: string;

  typeId: number;
  type: ResourceIdentifierType;
}
