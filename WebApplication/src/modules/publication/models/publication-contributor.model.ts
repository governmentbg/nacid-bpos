import { ContributorType } from 'src/modules/nomenclature/models/contributor-type.model';
import { NameType } from '../enums/name-type.enum';
import { PublicationContributorIdentifier } from './publication-contributor-identifier.model';
import { PublicationEntity } from './publication-entity.model';

export class PublicationContributor extends PublicationEntity {
  typeId: number;
  type: ContributorType;

  nameType: NameType;

  firstName: string;
  lastName: string;

  institutionAffiliationName: string;

  identifiers: PublicationContributorIdentifier[];

  constructor() {
    super();
    this.nameType = NameType.personal;
  }
}
