import { PublicationCreator } from './publication-creator.model';

export class PublicationCreatorAffiliation {
  id: number;

  publicationCreatorId: number;
  publicationCreator: PublicationCreator;

  institutionName: string;

  viewOrder: number | null;
}
