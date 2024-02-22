import { OrganizationalIdentifierScheme } from 'src/modules/nomenclature/models/organizational-identifier-scheme.model';
import { PublicationEntity } from './publication-entity.model';

export class PublicationFundingReference extends PublicationEntity {
  name: string;

  identifier: string;
  schemeId: number | null;
  scheme: OrganizationalIdentifierScheme;

  awardNumber: string;
  awardURI: string;
  awardTitle: string;
}
