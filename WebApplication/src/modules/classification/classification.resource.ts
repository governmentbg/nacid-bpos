import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseResource } from 'src/infrastructure/base/base.resource';
import { Configuration } from 'src/infrastructure/base/configuration/configuration';
import { ClassificationDto } from './dtos/classification.dto';
import { Classification } from './models/classification.model';

@Injectable()
export class ClassificationResource extends BaseResource {
  constructor(
    protected http: HttpClient,
    protected configuration: Configuration
  ) {
    super(http, configuration, 'Classification');
  }

  getClassificationRoots(): Observable<ClassificationDto[]> {
    return this.http.get<ClassificationDto[]>(`${this.baseUrl}/Roots`);
  }

  getChildClassifications(rootId: number): Observable<ClassificationDto[]> {
    return this.http.get<ClassificationDto[]>(`${this.baseUrl}/Roots/${rootId}`);
  }

  getById(id: number): Observable<Classification> {
    return this.http.get<Classification>(`${this.baseUrl}/${id}`);
  }

  createClassification(model: Classification): Observable<ClassificationDto> {
    return this.http.post<ClassificationDto>(this.baseUrl, model);
  }

  updateClassification(model: Classification): Observable<ClassificationDto> {
    return this.http.put<ClassificationDto>(`${this.baseUrl}/${model.id}`, model);
  }

  deleteClassification(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
