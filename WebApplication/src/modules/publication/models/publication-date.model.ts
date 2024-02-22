import { DateType } from '../enums/date-type.enum';
import { PublicationEntity } from './publication-entity.model';

export class PublicationDate extends PublicationEntity {
  value: Date;
  type: DateType;
  note: string;
}
