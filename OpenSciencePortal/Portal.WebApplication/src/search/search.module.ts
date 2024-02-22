import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { NgBootstrapModule } from 'src/infrastructure/ng-bootstrap.module';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { ComponentsModule } from 'src/components/components.module';

import { SearchComponent } from './components/search.component';
import { SearchResultComponent } from './components/search-result.component';
import { SearchRoutingModule } from './search-routing.module';
import { SearchResource } from './search.resource';
import { SearchFilterComponent } from './components/search-filter.component';
import { SearchFilterSidebarComponent } from './components/search-filter-sidebar.component';
import { SearchFilterService } from './search-filter.service';
import { SearchStateService } from './search.state';
import { LandingPageComponent } from './components/landing-page/landing-page.component';
import { SearchCategoriesComponent } from './components/search-categories/search-categories.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    SearchRoutingModule,
    TranslateModule,
    NgBootstrapModule,
    ComponentsModule,
    FontAwesomeModule
  ],
  declarations: [
    SearchComponent,
    SearchResultComponent,
    SearchFilterComponent,
    SearchFilterSidebarComponent,
    LandingPageComponent,
    SearchCategoriesComponent,
  ],
  providers: [
    SearchResource,
    SearchFilterService,
    SearchStateService
  ]
})
export class SearchModule { }
