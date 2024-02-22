import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { NomenclatureSelectModule } from 'src/infrastructure/components/nomenclature-select/nomenclature-select.module';
import { MaterialComponentsModule } from 'src/infrastructure/material-components.module';
import { MetadataHarvestingService } from 'src/infrastructure/services/metadata-harvesting.service';
import { SharedModule } from 'src/infrastructure/shared.module';
import { ClassificationRoutingModule } from './classification-routing.module';
import { ClassificationResource } from './classification.resource';
import { ClassificationChildrenComponent } from './components/classification-children.component';
import { ClassificationCreationModal } from './components/classification-creation-modal.component';
import { ClassificationHarvestingDataComponent } from './components/classification-harvesting-data.component';
import { ClassificationPublicationsComponent } from './components/classification-publications.component';
import { ClassificationViewerComponent } from './components/classification-viewer.component';
import { ClassificationsComponent } from './components/classifications.component';

@NgModule({
  imports: [
    CommonModule,
    SharedModule,
    FormsModule,
    MaterialComponentsModule,
    NomenclatureSelectModule,
    ClassificationRoutingModule,
    TranslateModule
  ],
  declarations: [
    ClassificationsComponent,
    ClassificationViewerComponent,
    ClassificationChildrenComponent,
    ClassificationPublicationsComponent,
    ClassificationCreationModal,
    ClassificationHarvestingDataComponent
  ],
  providers: [
    ClassificationResource,
    MetadataHarvestingService
  ],
  entryComponents: [
    ClassificationCreationModal
  ]
})
export class ClassificationModule { }
