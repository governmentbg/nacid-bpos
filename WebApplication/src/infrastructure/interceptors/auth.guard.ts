import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { UserLoginService } from 'src/modules/user/services/user-login.service';
import { Configuration } from '../base/configuration/configuration';

@Injectable()
export class AuthGuard implements CanActivate {
  constructor(
    private router: Router,
    private config: Configuration,
    private userService: UserLoginService
  ) { }

  canActivate(_: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    const token = localStorage.getItem(this.config.tokenProperty);
    const userId = localStorage.getItem(this.config.userIdProperty);
    if (token && userId) {
      return true;
    }

    this.userService.logout();
    this.router.navigate(['login']);
    return false;
  }
}
