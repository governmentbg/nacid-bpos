import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseResource } from 'src/infrastructure/base/base.resource';
import { Configuration } from 'src/infrastructure/base/configuration/configuration';
import { Nomenclature } from 'src/infrastructure/base/models/nomenclature.model';
import { BaseNomenclatureFilterDto } from './dtos/base-nomenclature-filter.dto';

@Injectable()
export class NomenclatureResource<T extends Nomenclature> extends BaseResource {
  constructor(
    protected http: HttpClient,
    protected configuration: Configuration
  ) {
    super(http, configuration);
  }

  getFiltered(filter?: BaseNomenclatureFilterDto): Observable<T[]> {
    return this.http.get<T[]>(`${this.getBaseUrl()}${this.composeQueryString(filter)}`);
  }

  add(entity: T): Observable<T> {
    return this.http.post<T>(this.baseUrl, entity);
  }

  update(id: number, entity: T): Observable<T> {
    return this.http.put<T>(`${this.baseUrl}/${id}`, entity);
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${id}`);
  }
}
