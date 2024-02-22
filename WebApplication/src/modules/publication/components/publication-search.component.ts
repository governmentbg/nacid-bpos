import { Component } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { BaseEntitySearchComponent } from 'src/infrastructure/components/base-entity-search.component';
import { LoadingIndicatorService } from 'src/infrastructure/components/loading-indicator/loading-indicator.service';
import { PublicationStatus } from '../enums/publication-status.enum';
import { Publication } from '../models/publication.model';
import { PublicationResource } from '../publication.resource';
import { PublicationSearchFilterService } from '../services/publication-search-filter.service';

@Component({
  selector: 'publication-search',
  templateUrl: 'publication-search.component.html',
  styleUrls: ['./publication-search.styles.css']
})

export class PublicationSearchComponent extends BaseEntitySearchComponent<Publication>{
  publicationStatuses = PublicationStatus;

  displayedColumns = ['actions', 'titles', 'authors', 'status', 'modificationDate'];

  constructor(
    public filter: PublicationSearchFilterService,
    protected resource: PublicationResource,
    protected loadingIndicator: LoadingIndicatorService,
    public translateService: TranslateService
  ) {
    super(filter, resource, loadingIndicator);
  }
}
