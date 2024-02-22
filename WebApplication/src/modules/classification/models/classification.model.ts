import { Entity } from 'src/infrastructure/base/models/entity.model';
import { Institution } from 'src/modules/institution/models/institution.model';
import { AccessRight } from 'src/modules/nomenclature/models/access-right.model';
import { LicenseType } from 'src/modules/nomenclature/models/license-type.model';
import { ResourceIdentifierType } from 'src/modules/nomenclature/models/resource-identifier-type.model';
import { ResourceType } from 'src/modules/nomenclature/models/resource-type.model';

export class Classification extends Entity {
  name: string;

  parentId: number | null;
  parent: Classification;

  organizationId: number | null;
  organization: Institution;

  isReadonly: boolean;
  harvestUrl: string;
  metadataFormat: string;
  sets: string[] = [];

  isOpenAirePropagationEnabled: boolean;

  defaultResourceTypeId: number | null;
  defaultResourceType: ResourceType;

  defaultIdentifierTypeId: number | null;
  defaultIdentifierType: ResourceIdentifierType;

  defaultAccessRightId: number | null;
  defaultAccessRight: AccessRight;

  defaultLicenseConditionId: number | null;
  defaultLicenseCondition: LicenseType;

  defaultLicenseStartDate: Date | null;

  children: Classification[];

  // For view purposes only
  isExpanded: boolean;

  constructor() {
    super();

    this.isReadonly = false;
    this.isOpenAirePropagationEnabled = false;
  }
}
