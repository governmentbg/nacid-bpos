import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSlideToggleChange } from '@angular/material/slide-toggle';
import { TranslateService } from '@ngx-translate/core';
import { ClassificationResource } from '../classification.resource';
import { ClassificationDto } from '../dtos/classification.dto';
import { IClassificationCreation } from '../interfaces/classification-creation.interface';
import { Classification } from '../models/classification.model';

@Component({
  selector: 'classification-creation-modal',
  templateUrl: 'classification-creation-modal.component.html'
})

export class ClassificationCreationModal {
  parentClassification: ClassificationDto;
  classification: Classification = new Classification();
  isNew = false;
  isValidClassificationHarvestingData = true;

  constructor(
    @Inject(MAT_DIALOG_DATA) private data: IClassificationCreation,
    private resource: ClassificationResource,
    public translateService: TranslateService
  ) {
    if (this.data) {
      if (this.data.id) {
        this.resource.getById(this.data.id)
          .subscribe((classification: Classification) => {
            this.classification = classification;
            if (classification.parent) {
              this.parentClassification = new ClassificationDto();
              this.parentClassification.id = classification.parent.id;
              this.parentClassification.name = classification.parent.name;
              this.parentClassification.organizationId = classification.parent.organizationId;
              if (classification.parent.organizationId) {
                this.parentClassification.organizationName = classification.parent.organization.name;
              }
            }
            this.isNew = false;
          });
      }
      else {
        this.isNew = true;
        this.parentClassification = this.data.parentClassification;
        if (this.parentClassification) {
          this.classification.parentId = this.parentClassification.id;
          this.classification.organizationId = this.parentClassification.organizationId;
        }
      }
    }
  }

  isFromHarvestingChanged(isFromHarvestingChange: MatSlideToggleChange) {
    this.classification.isReadonly = isFromHarvestingChange.checked;
    this.classification.isOpenAirePropagationEnabled = !this.classification.isReadonly;

    if (!this.classification.isReadonly) {
      this.isValidClassificationHarvestingData = true;
      this.classification.harvestUrl = undefined;
      this.classification.metadataFormat = undefined;
      this.classification.sets = [];

      this.classification.defaultResourceTypeId = null;
      this.classification.defaultResourceType = null;

      this.classification.defaultAccessRightId = null;
      this.classification.defaultAccessRight = null;

      this.classification.defaultIdentifierTypeId = null;
      this.classification.defaultIdentifierType = null;

      this.classification.defaultLicenseConditionId = null;
      this.classification.defaultLicenseCondition = null;

      this.classification.defaultLicenseStartDate = null;
    }
  }
}
