import { Entity } from 'src/infrastructure/base/models/entity.model';
import { Institution } from 'src/modules/institution/models/institution.model';
import { AccessRight } from 'src/modules/nomenclature/models/access-right.model';
import { LicenseType } from 'src/modules/nomenclature/models/license-type.model';
import { ResourceIdentifierType } from 'src/modules/nomenclature/models/resource-identifier-type.model';
import { ResourceType } from 'src/modules/nomenclature/models/resource-type.model';
import { PublicationStatus } from '../enums/publication-status.enum';
import { PublicationAlternateIdentifier } from './publication-alternate-identifier.model';
import { PublicationAudience } from './publication-audience.model';
import { PublicationClassification } from './publication-classification.model';
import { PublicationContributor } from './publication-contributor.model';
import { PublicationCoverage } from './publication-coverage.model';
import { PublicationCreator } from './publication-creator.model';
import { PublicationDate } from './publication-date.model';
import { PublicationDescription } from './publication-description.model';
import { PublicationFile } from './publication-file.model';
import { PublicationFormat } from './publication-format.model';
import { PublicationFundingReference } from './publication-funding-reference.model';
import { PublicationLanguage } from './publication-language.model';
import { PublicationPublisher } from './publication-publisher.model';
import { PublicationRelatedIdentifier } from './publication-related-identifier.model';
import { PublicationSize } from './publication-size.model';
import { PublicationSource } from './publication-source.model';
import { PublicationSubject } from './publication-subject.model';
import { PublicationTitle } from './publication-title.model';

export class Publication extends Entity {
  classifications: PublicationClassification[];

  titles: PublicationTitle[];
  creators: PublicationCreator[];
  contributors: PublicationContributor[];

  fundingReferences: PublicationFundingReference[];

  alternateIdentifiers: PublicationAlternateIdentifier[];
  relatedIdentifiers: PublicationRelatedIdentifier[];

  embargoPeriodStart: Date | null;
  embargoPeriodEnd: Date | null;

  languages: PublicationLanguage[];

  publishers: PublicationPublisher[];

  publishYear: number;
  publishMonth: number | null;
  publishDay: number | null;

  resourceTypeId: number | null;
  resourceType: ResourceType;

  descriptions: PublicationDescription[];

  formats: PublicationFormat[];

  identifier: string;
  identifierTypeId: number | null;
  identifierType: ResourceIdentifierType;

  sources: PublicationSource[];

  subjects: PublicationSubject[];

  accessRightId: number | null;
  accessRight: AccessRight;

  licenseTypeId: number | null;
  licenseType: LicenseType;

  otherLicenseCondition: string;
  otherLicenseUrl: string;

  licenseStartDate: Date | null;

  sizes: PublicationSize[];

  coverages: PublicationCoverage[];

  publicationVersion: string;
  publicationVersionURI: string;

  resourceVersion: string;
  resourceVersionURI: string;

  citationTitle: string;
  citationVolume: string;
  citationIssue: string;
  citationStartPage: number | null;
  citationEndPage: number | null;
  citationEdition: number | null;
  citationConferencePlace: string;
  citationConferenceStartDate: Date | null;
  citationConferenceEndDate: Date | null;

  files: PublicationFile[];

  audiences: PublicationAudience[];

  otherDates: PublicationDate[];

  status: PublicationStatus;

  moderatorInstitutionId: number;
  moderatorInstitution: Institution;

  createdByUserId: number;

  constructor() {
    super();
    this.titles = [new PublicationTitle()];
    this.creators = [new PublicationCreator()];
    this.status = PublicationStatus.draft;
  }
}
