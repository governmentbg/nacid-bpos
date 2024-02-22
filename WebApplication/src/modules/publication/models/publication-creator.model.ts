import { PublicationCreatorAffiliation } from './publication-creator-affiliation.model';
import { PublicationCreatorIdentifier } from './publication-creator-identifier.model';
import { PublicationEntity } from './publication-entity.model';

export class PublicationCreator extends PublicationEntity {
  firstName: string;
  lastName: string;

  language: string;

  identifiers: PublicationCreatorIdentifier[];
  affiliations: PublicationCreatorAffiliation[];
}
