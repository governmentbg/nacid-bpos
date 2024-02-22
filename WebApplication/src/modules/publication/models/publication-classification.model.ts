import { Classification } from 'src/modules/classification/models/classification.model';
import { PublicationEntity } from './publication-entity.model';

export class PublicationClassification extends PublicationEntity {
  classificationId: number;
  classification: Classification;
}
