import { Injectable } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { DomSanitizer } from '@angular/platform-browser';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Publication } from '../models/publication';
import { of, EMPTY } from 'rxjs';
import { Configuration } from '../../infrastructure/configuration/configuration';
import { LoadingIndicatorService } from '../../infrastructure/loading-indicator.service';
import { take, mergeMap } from 'rxjs/operators';

@Injectable()
export class PublicationViewResolverService implements Resolve<Publication> {
  constructor(
    private http: HttpClient,
    private router: Router,
    private config: Configuration,
    private loadingIndicator: LoadingIndicatorService,
    private sanitizer: DomSanitizer
  ) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    let id = route.paramMap.get('id');
    this.loadingIndicator.start();
    return this.http
      .get<Publication>(`${this.config.restUrl}/publication/${id}`)
      .pipe(
        take(1),
        mergeMap(publication => {
          this.loadingIndicator.stop();
          if (publication) {
            publication.descriptions = publication.descriptions
              .flatMap((descriptions) => descriptions.split('\n'));
            publication.files.forEach(file => {
              file.url = this.sanitizer.bypassSecurityTrustUrl(`${this.config.internalAppUrl}/api/FilesStorage?key=${file.key}&fileName=${file.name}&dbId=${file.dbId}`);
            });
            return of(publication);
          }
          else
            this.router.navigate(['notFound'])
          return EMPTY;
        })
      );
  }
}