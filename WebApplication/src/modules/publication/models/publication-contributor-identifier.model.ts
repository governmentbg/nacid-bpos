import { NameIdentifierScheme } from 'src/modules/nomenclature/models/name-identifier-scheme.model';
import { OrganizationalIdentifierScheme } from 'src/modules/nomenclature/models/organizational-identifier-scheme.model';
import { PublicationContributor } from './publication-contributor.model';

export class PublicationContributorIdentifier {
  id: number;

  publicationContributorId: number;
  publicationContributor: PublicationContributor;

  value: string;

  schemeId: number | null;
  scheme: NameIdentifierScheme;

  organizationalSchemeId: number | null;
  organizationalScheme: OrganizationalIdentifierScheme;

  viewOrder: number | null;
}
