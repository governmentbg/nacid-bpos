import { EventEmitter, Injectable } from '@angular/core';
import { Configuration } from 'src/infrastructure/base/configuration/configuration';
import { UserLoginInfoDto } from '../dtos/user-login-info.dto';
import { UserLoginEventEnum } from '../enums/user-login-event.enum';

@Injectable()
export class UserLoginService {
  private emitter: EventEmitter<{ event: UserLoginEventEnum }> = new EventEmitter();

  public isLogged = false;

  constructor(private configuration: Configuration) {
    this.isLogged = localStorage.getItem(this.configuration.tokenProperty) ? true : false;
  }

  login(userLoginInfoDto: UserLoginInfoDto) {
    localStorage.setItem(this.configuration.tokenProperty, userLoginInfoDto.token);
    localStorage.setItem(this.configuration.userIdProperty, userLoginInfoDto.id.toString());
    localStorage.setItem(this.configuration.userFullnameProperty, userLoginInfoDto.fullname);
    localStorage.setItem(this.configuration.userRolesProperty, userLoginInfoDto.roleAlias);

    if (userLoginInfoDto.institutionIds) {
      localStorage.setItem(this.configuration.userInstitutionsProperty, userLoginInfoDto.institutionIds.toString());
    }

    this.isLogged = true;
    this.emitter.emit({ event: UserLoginEventEnum.login });
  }

  logout() {
    localStorage.clear();
    this.isLogged = false;

    this.emitter.emit({ event: UserLoginEventEnum.logout });
  }

  subscribe(next: (value: { event: UserLoginEventEnum }) => void) {
    return this.emitter.subscribe(next);
  }
}
