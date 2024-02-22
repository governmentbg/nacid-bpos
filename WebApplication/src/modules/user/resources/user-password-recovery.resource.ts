import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Configuration } from 'src/infrastructure/base/configuration/configuration';
import { ForgottenPasswordRecoveryDto } from '../dtos/forgotten-password-recovery.dto';
import { ForgottenPasswordDto } from '../dtos/forgotten-password.dto';

@Injectable()
export class UserPasswordRecoveryResource {
  constructor(
    private http: HttpClient,
    private configuration: Configuration
  ) { }

  sendForgottenPassword(model: ForgottenPasswordDto): Observable<void> {
    return this.http.post<void>(`${this.configuration.restUrl}/ForgottenPassword`, model);
  }

  recoverPassword(model: ForgottenPasswordRecoveryDto): Observable<void> {
    return this.http.post<void>(`${this.configuration.restUrl}/ForgottenPassword/Recovery`, model);
  }
}
