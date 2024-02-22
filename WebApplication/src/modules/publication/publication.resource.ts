import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseEntityResource } from 'src/infrastructure/base/base-entity.resource';
import { Configuration } from 'src/infrastructure/base/configuration/configuration';
import { PublicationSearchDto } from './dtos/publication-search-result.dto';
import { Publication } from './models/publication.model';
import { PublicationSearchFilterService } from './services/publication-search-filter.service';

@Injectable()
export class PublicationResource extends BaseEntityResource<Publication> {
  constructor(
    protected http: HttpClient,
    protected configuration: Configuration
  ) {
    super(http, configuration, 'Publication');
  }

  getFiltered(filter: PublicationSearchFilterService): Observable<PublicationSearchDto[]> {
    return this.http.get<PublicationSearchDto[]>(`${this.baseUrl}${this.composeQueryString(filter)}`);
  }

  markPending(id: number): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/${id}/PendingApproval`, null);
  }

  publish(id: number): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/${id}/Publishing`, null);
  }

  deny(id: number): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/${id}/Denial`, null);
  }
}
