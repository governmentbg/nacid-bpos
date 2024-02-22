import { Injectable } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Publication } from '../models/publication';
import { of, EMPTY } from 'rxjs';
import { Configuration } from 'src/infrastructure/configuration/configuration';
import { take, mergeMap } from 'rxjs/operators';
import { LoadingIndicatorService } from 'src/infrastructure/loading-indicator.service';

@Injectable()
export class PublicationDetailResolverService implements Resolve<Publication> {
  constructor(
    private http: HttpClient,
    private router: Router,
    private config: Configuration,
    private loadingIndicator: LoadingIndicatorService
  ) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    let id = route.paramMap.get('id');
    this.loadingIndicator.start();
    return this.http
      .get<Publication>(`${this.config.restUrl}/publication/${id}/details`)
      .pipe(
        take(1),
        mergeMap(publication => {
          this.loadingIndicator.stop();
          if (publication) {
            return of(publication);
          }
          else
            this.router.navigate(['notFound'])
          return EMPTY;
        })
      );
  }
}