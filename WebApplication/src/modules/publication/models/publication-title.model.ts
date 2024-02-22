import { Language } from 'src/modules/nomenclature/models/language.model';
import { TitleType } from 'src/modules/nomenclature/models/title-type.model';
import { PublicationEntity } from './publication-entity.model';

export class PublicationTitle extends PublicationEntity {
  value: string;

  typeId: number;
  type: TitleType;

  languageId: number | null;
  language: Language;
}
