import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseResource } from 'src/infrastructure/base.resource';
import { Configuration } from 'src/infrastructure/configuration/configuration';
import { SearchResult } from './models/search-result';
import { Observable } from 'rxjs';

@Injectable()
export class SearchResource extends BaseResource {
  constructor(
    protected http: HttpClient,
    protected configuration: Configuration
  ) {
    super(http, configuration, '');
  }

  search(search: any, aggregate?: any): Observable<SearchResult> {
    return this.http.post<SearchResult>(`${this.baseUrl}/search`, { search, aggregate });
  }

  aggregate(filter: any): Observable<SearchResult> {
    return this.http.post<SearchResult>(`${this.baseUrl}/aggregate`, filter);
  }

  get categories() {
    return this.http.get<any>(`${this.baseUrl}/categories`);
  }
}