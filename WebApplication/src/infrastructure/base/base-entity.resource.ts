import { Observable } from 'rxjs';
import { BaseResource } from './base.resource';
import { BaseEntityFilterService } from './interfaces/base-entity-filter.service';
import { Entity } from './models/entity.model';

export class BaseEntityResource<T extends Entity> extends BaseResource {
  getFiltered(filter?: BaseEntityFilterService): Observable<any[]> {
    return this.http.get<T[]>(`${this.baseUrl}${this.composeQueryString(filter)}`);
  }

  getById(id: number): Observable<T> {
    return this.http.get<T>(`${this.baseUrl}/${id}`);
  }

  post(entity: T): Observable<T> {
    return this.http.post<T>(this.baseUrl, entity);
  }

  update(id: number, entity: T): Observable<T> {
    return this.http.put<T>(`${this.baseUrl}/${id}`, entity);
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${id}`);
  }
}
