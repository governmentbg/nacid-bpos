import { RelationType } from 'src/modules/nomenclature/models/relation-type.model';
import { ResourceIdentifierType } from 'src/modules/nomenclature/models/resource-identifier-type.model';
import { ResourceTypeGeneral } from 'src/modules/nomenclature/models/resource-type-general.model';
import { PublicationEntity } from './publication-entity.model';

export class PublicationRelatedIdentifier extends PublicationEntity {
  value: string;

  typeId: number;
  type: ResourceIdentifierType;

  relationTypeId: number;
  relationType: RelationType;

  relatedMetadataScheme: string;
  schemeURI: string;
  schemeType: string;

  resourceTypeGeneralId: number | null;
  resourceTypeGeneral: ResourceTypeGeneral;
}
