import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MaterialComponentsModule } from 'src/infrastructure/material-components.module';
import { ViewOrderPipe } from 'src/infrastructure/pipes/view-order.pipe';
import { AccessRightsComponent } from './components/access-rights.component';
import { AudienceTypesComponent } from './components/audience-types.component';
import { ContributorTypesComponent } from './components/contributor-types.component';
import { DescriptionTypesComponent } from './components/description-types.component';
import { GeneralResourceTypesComponent } from './components/general-resource-types.component';
import { ResourceIdentifierTypesComponent } from './components/identifier-types.component';
import { LanguagesComponent } from './components/languages.component';
import { LicenseTypesComponent } from './components/license-types.component';
import { NameIdentifiersComponent } from './components/name-identifiers.component';
import { NomenclatureComponent } from './components/nomenclature.component';
import { OrganizationalIdentifiersComponent } from './components/organizational-identifiers.component';
import { RelationTypesComponent } from './components/relation-types.component';
import { ResourceTypesComponent } from './components/resource-types.component';
import { TitleTypesComponent } from './components/title-types.component';
import { NomenclatureRoutingModule } from './nomenclature-routing.module';
import { NomenclatureResource } from './nomenclature.resource';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    MaterialComponentsModule,
    NomenclatureRoutingModule
  ],
  declarations: [
    NomenclatureComponent,
    ResourceTypesComponent,
    GeneralResourceTypesComponent,
    ResourceIdentifierTypesComponent,
    NameIdentifiersComponent,
    OrganizationalIdentifiersComponent,
    RelationTypesComponent,
    TitleTypesComponent,
    LanguagesComponent,
    ContributorTypesComponent,
    DescriptionTypesComponent,
    LicenseTypesComponent,
    AccessRightsComponent,
    AudienceTypesComponent,
    ViewOrderPipe
  ],
  providers: [
    NomenclatureResource
  ]
})
export class NomenclatureModule { }
