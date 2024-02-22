import { Publication } from './publication.model';

export class PublicationEntity {
  publicationId: number;
  publication: Publication;

  viewOrder: number;

  constructor() {
    this.viewOrder = 1;
  }
}
