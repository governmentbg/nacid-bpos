import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from 'src/infrastructure/interceptors/auth.guard';
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

const routes: Routes = [
  {
    path: 'nomenclatures',
    component: NomenclatureComponent,
    children: [
      {
        path: 'resourceTypes',
        component: ResourceTypesComponent,
        canActivate: [AuthGuard]
      },
      {
        path: 'generalResourceTypes',
        component: GeneralResourceTypesComponent,
        canActivate: [AuthGuard]
      },
      {
        path: 'resourceIdentifierTypes',
        component: ResourceIdentifierTypesComponent,
        canActivate: [AuthGuard]
      },
      {
        path: 'nameIdentifiers',
        component: NameIdentifiersComponent,
        canActivate: [AuthGuard]
      },
      {
        path: 'relationTypes',
        component: RelationTypesComponent,
        canActivate: [AuthGuard]
      },
      {
        path: 'organizationalIdentifiers',
        component: OrganizationalIdentifiersComponent,
        canActivate: [AuthGuard]
      },
      {
        path: 'titleTypes',
        component: TitleTypesComponent,
        canActivate: [AuthGuard]
      },
      {
        path: 'languages',
        component: LanguagesComponent,
        canActivate: [AuthGuard]
      },
      {
        path: 'contributorTypes',
        component: ContributorTypesComponent,
        canActivate: [AuthGuard]
      },
      {
        path: 'descriptionTypes',
        component: DescriptionTypesComponent,
        canActivate: [AuthGuard]
      },
      {
        path: 'licenseTypes',
        component: LicenseTypesComponent,
        canActivate: [AuthGuard]
      },
      {
        path: 'accessRights',
        component: AccessRightsComponent,
        canActivate: [AuthGuard]
      },
      {
        path: 'audienceTypes',
        component: AudienceTypesComponent,
        canActivate: [AuthGuard]
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class NomenclatureRoutingModule { }
