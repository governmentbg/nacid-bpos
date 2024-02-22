import { PublicationStatus } from '../enums/publication-status.enum';

export class PublicationSearchDto {
  id: number;

  titles: string[];
  authors: string[];

  modificationDate: Date;

  status: PublicationStatus;
}
