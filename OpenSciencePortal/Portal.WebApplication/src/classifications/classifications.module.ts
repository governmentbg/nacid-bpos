import { NgModule } from "@angular/core";
import { OrganizationComponent } from './components/organization/organization.component';
import { ScientificAreaComponent } from './components/scientific-area/scientific-area.component';
import { ClassificationsRouting } from './classifications.routing';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { ScientificAreasDataResolverService } from './services/scientific-area-data-resolver.service';
import { OrganizationsDataResolverService } from './services/organization-data-resolver.service';
import { OrganizationViewComponent } from './components/organization-view/organization-view.component';
import { OrganizationViewDataResolverService } from './services/organization-view-data-resolver.service';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';

@NgModule({
  imports: [
    CommonModule,
    TranslateModule,
    FontAwesomeModule,
    ClassificationsRouting
  ],
  declarations: [
    OrganizationComponent,
    ScientificAreaComponent,
    OrganizationViewComponent
  ],
  providers: [
    ScientificAreasDataResolverService,
    OrganizationsDataResolverService,
    OrganizationViewDataResolverService
  ]
})
export class ClassificationsModule { }