import { Injectable } from "@angular/core";
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Configuration } from 'src/infrastructure/configuration/configuration';
import { take, mergeMap } from 'rxjs/operators';
import { of, EMPTY } from 'rxjs';
import { LoadingIndicatorService } from 'src/infrastructure/loading-indicator.service';
import { SearchFilterService } from 'src/search/search-filter.service';
import { SearchFieldCategories, SearchFieldType } from 'src/search/models/search-field-type.enum';
import { Classification } from '../models/classification';
import { SEARCH_FIELD_SPEC } from 'src/search/models/search-field-type-spec';

@Injectable()
export class OrganizationViewDataResolverService implements Resolve<Classification> {

  constructor(
    private http: HttpClient,
    private config: Configuration,
    private router: Router,
    private loadingIndicator: LoadingIndicatorService
  ) { }

  resolve(route: ActivatedRouteSnapshot) {
    let id = route.paramMap.get('id');
    let classificationsSpec = SEARCH_FIELD_SPEC[SearchFieldType.classifications];

    let query = {
      aggregations: {
        [classificationsSpec.name]: {
          terms: {
            field: classificationsSpec.name,
            size: 100000
          }
        }
      },
      size: 0
    };

    this.loadingIndicator.start();
    return this.http
      .post<Classification>(`${this.config.restUrl}/institution/hierarchy/${id}`, query)
      .pipe(
        take(1),
        mergeMap((institution: Classification) => {
          this.loadingIndicator.stop();
          if (institution) {
            institution.children.forEach(classification => classification.isOpen = true);
            return of(institution);
          }
          else {
            this.router.navigate(['/notFound']);
            return EMPTY;
          }
        })
      );
  }
}