import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { MaterialComponentsModule } from 'src/infrastructure/material-components.module';
import { InstitutionEditComponent } from './components/institution-edit.component';
import { InstitutionSearchComponent } from './components/institution-search.component';
import { InstitutionRoutingModule } from './institution-routing.module';
import { InstitutionResource } from './institution.resource';
import { InstitutionSearchFilterService } from './services/institution-search-filter.service';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    MaterialComponentsModule,
    InstitutionRoutingModule,
    TranslateModule
  ],
  declarations: [
    InstitutionSearchComponent,
    InstitutionEditComponent
  ],
  providers: [
    InstitutionSearchFilterService,
    InstitutionResource
  ]
})
export class InstitutionModule { }
