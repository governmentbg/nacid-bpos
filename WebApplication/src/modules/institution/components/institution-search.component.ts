import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { BaseEntitySearchComponent } from 'src/infrastructure/components/base-entity-search.component';
import { LoadingIndicatorService } from 'src/infrastructure/components/loading-indicator/loading-indicator.service';
import { InstitutionResource } from '../institution.resource';
import { Institution } from '../models/institution.model';
import { InstitutionSearchFilterService } from '../services/institution-search-filter.service';

@Component({
  selector: 'institution-search',
  templateUrl: 'institution-search.component.html'
})

export class InstitutionSearchComponent extends BaseEntitySearchComponent<Institution> implements OnInit {
  displayedColumns: string[] = ['actions', 'name', 'repositoryUrl', 'areCommonClassificationsVisible', 'isActive'];

  constructor(
    public filter: InstitutionSearchFilterService,
    protected resource: InstitutionResource,
    protected loadingIndicator: LoadingIndicatorService,
    public translateService: TranslateService
  ) {
    super(filter, resource, loadingIndicator);
  }

  ngOnInit() {
    this.translateService.onLangChange.subscribe(() => this.search());

    super.ngOnInit();
  }

  search(loadMore?: boolean) {
    this.filter.searchInForeignNameOnly = this.translateService.currentLang !== this.translateService.defaultLang
    super.search(loadMore);
  }
}
