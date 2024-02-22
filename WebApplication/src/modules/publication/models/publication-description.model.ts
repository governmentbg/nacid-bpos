import { Language } from 'src/modules/nomenclature/models/language.model';
import { PublicationEntity } from './publication-entity.model';

export class PublicationDescription extends PublicationEntity {
  value: string;

  languageId: number | null;
  language: Language;
}
