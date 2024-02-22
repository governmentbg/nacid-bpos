import { Language } from 'src/modules/nomenclature/models/language.model';
import { PublicationEntity } from './publication-entity.model';

export class PublicationLanguage extends PublicationEntity {
  languageId: number;
  language: Language;
}
