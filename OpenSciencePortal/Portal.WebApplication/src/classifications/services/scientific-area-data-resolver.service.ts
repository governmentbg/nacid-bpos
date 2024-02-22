import { Injectable } from "@angular/core";
import { Resolve, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Configuration } from 'src/infrastructure/configuration/configuration';
import { take, mergeMap } from 'rxjs/operators';
import { of, EMPTY } from 'rxjs';
import { LoadingIndicatorService } from 'src/infrastructure/loading-indicator.service';
import { SearchFieldType } from 'src/search/models/search-field-type.enum';
import { Classification } from '../models/classification';
import { SEARCH_FIELD_SPEC } from 'src/search/models/search-field-type-spec';

@Injectable()
export class ScientificAreasDataResolverService implements Resolve<Classification[]> {

  constructor(
    private http: HttpClient,
    private config: Configuration,
    private router: Router,
    private loadingIndicator: LoadingIndicatorService
  ) { }

  resolve() {
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
      .post<Classification[]>(`${this.config.restUrl}/classification/hierarchy`, query)
      .pipe(
        take(1),
        mergeMap(hierarchy => {
          this.loadingIndicator.stop();
          if (hierarchy && hierarchy.length) {
            return of(hierarchy);
          }
          else {
            this.router.navigate(['/notFound']);
            return EMPTY;
          }
        })
      );
  }
}