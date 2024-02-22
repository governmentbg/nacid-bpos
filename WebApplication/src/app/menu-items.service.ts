import { Injectable } from '@angular/core';
import { IMenuItem } from 'src/infrastructure/components/menu/interfaces/menu-item.interface';
import { UserRoleAliases } from 'src/modules/user/constants/user-role-aliases.constant';
import { UserRoleService } from 'src/modules/user/services/user-role.service';

@Injectable()
export class MenuItemService {
  constructor(private userRoleService: UserRoleService) { }

  getMenuItems(isLoggedInUser: boolean): IMenuItem[] {
    return [
      {
        route: 'classifications',
        name: 'app.menu.classifications',
        isActive: this.userRoleService.hasRole(UserRoleAliases.ADMINISTRATOR)
          || this.userRoleService.hasRole(UserRoleAliases.ORGANIZATION_ADMINISTRATOR),
        isSelected: false
      },
      {
        route: 'publications',
        name: 'app.menu.publications',
        isActive: isLoggedInUser,
        isSelected: false
      },
      {
        route: 'institutions',
        name: 'app.menu.institutions',
        isActive: this.userRoleService.hasRole(UserRoleAliases.ADMINISTRATOR),
        isSelected: false
      },
      {
        route: 'nomenclatures',
        name: 'app.menu.nomenclatures',
        isActive: this.userRoleService.hasRole(UserRoleAliases.ADMINISTRATOR),
        isSelected: false
      },
      {
        route: 'users',
        name: 'app.menu.users',
        isActive: this.userRoleService.hasRole(UserRoleAliases.ADMINISTRATOR)
          || this.userRoleService.hasRole(UserRoleAliases.ORGANIZATION_ADMINISTRATOR),
        isSelected: false
      }
    ];
  }

  getFirstActive(): IMenuItem {
    const menuItems = this.getMenuItems(true);
    return menuItems.filter(e => e.isActive)[0];
  }
}
