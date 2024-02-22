import { Language } from 'src/modules/nomenclature/models/language.model';
import { PublicationEntity } from './publication-entity.model';

export class PublicationSubject extends PublicationEntity {
  value: string;

  languageId: number;
  language: Language;
}
