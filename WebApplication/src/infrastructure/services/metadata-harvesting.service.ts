import { BaseResource } from './../base/base.resource';
import { Injectable } from '@angular/core';
import { Configuration } from '../base/configuration/configuration';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class MetadataHarvestingService extends BaseResource {
  constructor(
    http: HttpClient,
    configuration: Configuration
  ) {
    super(http, configuration, 'MetadataHarvesting');
  }

  validateRepositoryUrl(url: string): Observable<boolean> {
    var params = { url };

    return this.http.get<boolean>(`${this.baseUrl}/ValidateRepositoryUrl`, { params });
  }

  getRepositorySets(url: string): Observable<{ name: string, spec: string }[]> {
    var params = { url };

    return this.http.get<{ name: string, spec: string }[]>(`${this.baseUrl}/ListSets`, { params });
  }
}
