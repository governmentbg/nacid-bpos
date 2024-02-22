import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Configuration } from 'src/infrastructure/base/configuration/configuration';
import { UserActivationDto } from '../dtos/user-activation.dto';

@Injectable()
export class UserActivationResource {
  constructor(
    private http: HttpClient,
    private configuration: Configuration
  ) { }

  activateUser(model: UserActivationDto): Observable<any> {
    return this.http.post(`${this.configuration.restUrl}/Activation`, model);
  }
}
