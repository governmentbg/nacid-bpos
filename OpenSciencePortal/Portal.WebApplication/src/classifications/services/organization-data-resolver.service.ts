import { Injectable } from "@angular/core";
import { Resolve, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Configuration } from 'src/infrastructure/configuration/configuration';
import { take, mergeMap } from 'rxjs/operators';
import { of, EMPTY } from 'rxjs';
import { LoadingIndicatorService } from 'src/infrastructure/loading-indicator.service';
import { SearchFilterService } from 'src/search/search-filter.service';
import { SearchFieldCategories } from 'src/search/models/search-field-type.enum';
import { Classification } from '../models/classification';



@Injectable()
export class OrganizationsDataResolverService implements Resolve<Classification[]> {

  constructor(
    private http: HttpClient,
    private config: Configuration,
    private router: Router,
    private loadingIndicator: LoadingIndicatorService
  ) { }

  resolve() {
    this.loadingIndicator.start();
    return this.http
      .get<Classification[]>(`${this.config.restUrl}/institution`)
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