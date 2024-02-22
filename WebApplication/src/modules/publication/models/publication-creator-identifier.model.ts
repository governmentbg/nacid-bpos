import { NameIdentifierScheme } from 'src/modules/nomenclature/models/name-identifier-scheme.model';
import { PublicationCreator } from './publication-creator.model';

export class PublicationCreatorIdentifier {
  id: number;

  publicationCreatorId: number;
  publicationCreator: PublicationCreator;

  value: string;
  schemeId: number;
  scheme: NameIdentifierScheme;

  viewOrder: number | null;
}
