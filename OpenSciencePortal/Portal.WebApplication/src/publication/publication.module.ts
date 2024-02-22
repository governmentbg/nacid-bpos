import { NgModule } from "@angular/core";

import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { TranslateModule } from '@ngx-translate/core';

import { PublicationViewComponent } from './components/publication-view/publication-view.component';
import { PublicationRouting } from './publication.routing';
import { PublicationViewResolverService } from './services/publication-view-resolver.service';
import { PublicationDetailComponent } from './components/publication-detail/publication-detail.component';
import { PublicationDetailResolverService } from './services/publication-detail-resolver.service';

@NgModule({
  imports: [
    CommonModule,
    HttpClientModule,
    TranslateModule,
    FontAwesomeModule,
    PublicationRouting
  ],
  declarations: [
    PublicationViewComponent,
    PublicationDetailComponent
  ],
  providers: [
    PublicationViewResolverService,
    PublicationDetailResolverService
  ]
})
export class PublicationModule { }