import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { FileSelectComponent } from 'src/infrastructure/components/file-upload/file-select.component';
import { NomenclatureSelectModule } from 'src/infrastructure/components/nomenclature-select/nomenclature-select.module';
import { MaterialComponentsModule } from 'src/infrastructure/material-components.module';
import { FileSizePipe } from 'src/infrastructure/pipes/file-size.pipe';
import { SharedModule } from 'src/infrastructure/shared.module';
import { ClassificationModule } from '../classification/classification.module';
import { PublicationAlternateIdentifiersComponent } from './components/publication-alternate-identifiers.component';
import { PublicationAudiencesComponent } from './components/publication-audiences.component';
import { PublicationContributorsComponent } from './components/publication-contributors.component';
import { PublicationCoveragesComponent } from './components/publication-coverages.component';
import { PublicationCreatorsComponent } from './components/publication-creators.component';
import { PublicationDescriptionsComponent } from './components/publication-descriptions.component';
import { PublicationFilesComponent } from './components/publication-files.component';
import { PublicationFormatsComponent } from './components/publication-formats.component';
import { PublicationFundingReferencesComponent } from './components/publication-funding-references.component';
import { PublicationLanguagesComponent } from './components/publication-languages.component';
import { PublicationPublishersComponent } from './components/publication-publishers.component';
import { PublicationRelatedIdentifiersComponent } from './components/publication-related-identifiers.component';
import { PublicationSearchComponent } from './components/publication-search.component';
import { PublicationSizesComponent } from './components/publication-sizes.component';
import { PublicationSourcesComponent } from './components/publication-sources.component';
import { PublicationStatusComponent } from './components/publication-status.component';
import { PublicationStepActionsComponent } from './components/publication-step-actions.component';
import { PublicationSubjectsComponent } from './components/publication-subjects.component';
import { PublicationTitlesComponent } from './components/publication-titles.component';
import { PublicationComponent } from './components/publication.component';
import { PublicationRoutingModule } from './publication-routing.module';
import { PublicationResource } from './publication.resource';
import { PublicationSearchFilterService } from './services/publication-search-filter.service';

@NgModule({
  imports: [
    CommonModule,
    SharedModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialComponentsModule,
    NomenclatureSelectModule,
    ClassificationModule,
    TranslateModule,
    PublicationRoutingModule
  ],
  declarations: [
    PublicationSearchComponent,
    PublicationStatusComponent,
    PublicationComponent,
    PublicationTitlesComponent,
    PublicationCreatorsComponent,
    PublicationContributorsComponent,
    PublicationDescriptionsComponent,
    PublicationLanguagesComponent,
    PublicationSubjectsComponent,
    PublicationFundingReferencesComponent,
    PublicationRelatedIdentifiersComponent,
    PublicationAlternateIdentifiersComponent,
    PublicationSourcesComponent,
    PublicationPublishersComponent,
    PublicationCoveragesComponent,
    PublicationAudiencesComponent,
    PublicationFormatsComponent,
    PublicationSizesComponent,
    PublicationStepActionsComponent,
    PublicationFilesComponent,
    FileSelectComponent,
    FileSizePipe
  ],
  providers: [
    PublicationSearchFilterService,
    PublicationResource
  ]
})
export class PublicationModule { }
