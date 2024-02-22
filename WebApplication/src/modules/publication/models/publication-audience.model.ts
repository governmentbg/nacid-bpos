import { AudienceType } from 'src/modules/nomenclature/models/audience-type.model';
import { PublicationEntity } from './publication-entity.model';

export class PublicationAudience extends PublicationEntity {
  typeId: number;
  type: AudienceType;
}
