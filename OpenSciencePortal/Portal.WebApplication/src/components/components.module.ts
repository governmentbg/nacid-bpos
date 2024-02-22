import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NgBootstrapModule } from 'src/infrastructure/ng-bootstrap.module';
import { TranslateModule } from '@ngx-translate/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';

import { SelectComponent } from './select/select.component';
import { AsyncSelectComponent } from './async-select/async-select.component';
import { CollapsableTextComponent } from './collapsable-text/collapsable-text.component';
import { LoadingIndicatorComponent } from './loading-indicator/loading-indicator.component';
import { PaginatorComponent } from './paginator/paginator.component';
import { LanguageSelectorComponent } from './language-selector/language-selector.component';
import { LightbulbIcon } from './icons/lightbulb.icon';
import { ShowMoreComponent } from './show-more/show-more.component';
import { DrawerComponent } from './drawer/drawer.component';
import { ForeignNameSelectorComponent } from './foreign-name-selector/foreign-name-selector.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NgBootstrapModule,
    TranslateModule,
    FontAwesomeModule
  ],
  declarations: [
    SelectComponent,
    AsyncSelectComponent,
    CollapsableTextComponent,
    LoadingIndicatorComponent,
    PaginatorComponent,
    LanguageSelectorComponent,
    LightbulbIcon,
    ShowMoreComponent,
    DrawerComponent,
    ForeignNameSelectorComponent
  ],
  exports: [
    SelectComponent,
    AsyncSelectComponent,
    CollapsableTextComponent,
    LoadingIndicatorComponent,
    PaginatorComponent,
    LanguageSelectorComponent,
    LightbulbIcon,
    ShowMoreComponent,
    DrawerComponent,
    ForeignNameSelectorComponent
  ]
})
export class ComponentsModule { }