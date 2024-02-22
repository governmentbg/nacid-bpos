import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { LanguageDto } from 'src/infrastructure/components/language-select/dtos/language.dto';
import { IMenuItem } from 'src/infrastructure/components/menu/interfaces/menu-item.interface';
import { UserLoginEventEnum } from 'src/modules/user/enums/user-login-event.enum';
import { UserLoginService } from 'src/modules/user/services/user-login.service';
import { UserRoleService } from 'src/modules/user/services/user-role.service';
import { MenuItemService } from './menu-items.service';

@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html'
})
export class AppComponent implements OnInit {
  isLoggedInUser = false;
  menuItems: IMenuItem[];

  appLanguages: LanguageDto[] = [
    {
      label: 'BG',
      alias: 'bg',
      imgSrc: './../assets/bulgaria-flag-round-icon-16.png',
      isUsed: false
    },
    {
      label: 'EN',
      alias: 'en',
      imgSrc: './../assets/united-kingdom-flag-round-icon-16.png',
      isUsed: false
    }
  ];

  constructor(
    private loginService: UserLoginService,
    private userRoleService: UserRoleService,
    private menuItemsService: MenuItemService,
    private router: Router,
    private translate: TranslateService
  ) {
    this.translate.setDefaultLang('bg');
    this.translate.use('bg');
  }

  ngOnInit() {
    this.isLoggedInUser = this.loginService.isLogged;
    this.menuItems = this.menuItemsService.getMenuItems(this.isLoggedInUser);

    this.loginService
      .subscribe(this.onLoggedInChange.bind(this));
  }

  private onLoggedInChange(loginEvent: { event: UserLoginEventEnum }) {
    if (loginEvent.event === UserLoginEventEnum.login) {
      this.userRoleService.getRole();
      this.router.navigate(['']);
    } else if (loginEvent.event === UserLoginEventEnum.logout) {
      this.userRoleService.clearRole();
      this.router.navigate(['login']);
    }

    this.isLoggedInUser = this.loginService.isLogged;
    this.menuItems = this.menuItemsService.getMenuItems(this.isLoggedInUser);
  }
}
