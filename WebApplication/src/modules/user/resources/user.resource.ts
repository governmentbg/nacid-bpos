import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseEntityResource } from 'src/infrastructure/base/base-entity.resource';
import { Configuration } from 'src/infrastructure/base/configuration/configuration';
import { UserChangePasswordDto } from '../dtos/user-change-password.dto';
import { UserCreationDto } from '../dtos/user-creation.dto';
import { UserSearchResultDto } from '../dtos/user-search-result.dto';
import { User } from '../models/user.model';
import { UserSearchFilterService } from '../services/user-search-filter.service';

@Injectable()
export class UserResource extends BaseEntityResource<User> {
  constructor(
    protected http: HttpClient,
    protected configuration: Configuration
  ) {
    super(http, configuration, 'User');
  }

  getFiltered(filter: UserSearchFilterService): Observable<UserSearchResultDto[]> {
    return this.http.get<UserSearchResultDto[]>(`${this.baseUrl}${this.composeQueryString(filter)}`);
  }

  createUser(model: UserCreationDto): Observable<any> {
    return this.http.post(`${this.baseUrl}/Creation`, model);
  }

  changePassword(model: UserChangePasswordDto): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/NewPassword`, model);
  }

  deactivate(userId: number): Observable<User> {
    return this.http.post<User>(`${this.baseUrl}/Deactivation/${userId}`, null);
  }

  sendActivationMail(userId: number): Observable<any> {
    return this.http.post(`${this.baseUrl}/Reactivation/${userId}`, null);
  }
}
