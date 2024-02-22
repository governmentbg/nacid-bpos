import { ChangeDetectionStrategy, Component, ElementRef, HostBinding, HostListener, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Configuration } from 'src/infrastructure/base/configuration/configuration';
import { UserChangePasswordModalComponent } from 'src/modules/user/components/user-change-password-modal.component';
import { UserLoginService } from 'src/modules/user/services/user-login.service';

@Component({
  selector: 'current-user',
  templateUrl: 'current-user.component.html',
  styleUrls: ['./current-user.styles.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})

export class CurrentUserComponent implements OnInit {
  user: any;

  constructor(
    private elRef: ElementRef,
    private configuration: Configuration,
    private loginService: UserLoginService,
    private dialog: MatDialog
  ) {
  }

  ngOnInit() {
    this.user = {
      name: localStorage.getItem(this.configuration.userFullnameProperty)
    };
  }

  @HostListener('document:click', ['$event']) onclick(event) {
    if (!this.elRef.nativeElement.contains(event.target) && this.shouldOpenUserMenu) {
      this.shouldOpenUserMenu = false;
    }
  }

  @HostBinding('class.highlighted') shouldOpenUserMenu: boolean;

  openUserMenu() {
    this.shouldOpenUserMenu = !this.shouldOpenUserMenu;
  }

  changePassword() {
    this.shouldOpenUserMenu = false;

    this.dialog.open(UserChangePasswordModalComponent, {
      disableClose: true
    })
      .afterClosed()
      .subscribe();
  }

  logout() {
    this.loginService.logout();
  }

}
