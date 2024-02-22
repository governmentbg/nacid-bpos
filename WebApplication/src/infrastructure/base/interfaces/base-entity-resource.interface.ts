import { Observable } from 'rxjs';
import { Entity } from '../models/entity.model';
import { BaseEntityFilterService } from './base-entity-filter.service';

export interface IBaseEntityResource<T extends Entity> {
  getFiltered(filter?: BaseEntityFilterService): Observable<any[]>;

  getById(id: number): Observable<T>;

  post(entity: T): Observable<T>;

  update(id: number, entity: T): Observable<T>;

  delete(id: number): Observable<void>;
}
