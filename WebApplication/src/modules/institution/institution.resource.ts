import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseEntityResource } from 'src/infrastructure/base/base-entity.resource';
import { Configuration } from 'src/infrastructure/base/configuration/configuration';
import { FlatClassificationHierarchyItemDto } from '../classification/dtos/flat-classification-hierarchy-item.dto';
import { Institution } from './models/institution.model';

@Injectable()
export class InstitutionResource extends BaseEntityResource<Institution> {
  constructor(
    protected http: HttpClient,
    protected configuration: Configuration
  ) {
    super(http, configuration, "Institution");
  }

  getInstitutionClassifications(institutionId: number): Observable<FlatClassificationHierarchyItemDto[]> {
    let url = `${this.baseUrl}/Classifications?institutionId=${institutionId}`;

    return this.http.get<FlatClassificationHierarchyItemDto[]>(url);
  }

  public deactivate(institutionId: number): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${institutionId}/Deactivation`, null);
  }
}
