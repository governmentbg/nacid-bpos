import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Configuration } from 'src/infrastructure/base/configuration/configuration';
import { DomainError } from 'src/infrastructure/base/models/domain-error.model';
import { LoadingIndicatorService } from 'src/infrastructure/components/loading-indicator/loading-indicator.service';
import { LoginDto } from '../dtos/login.dto';
import { UserLoginInfoDto } from '../dtos/user-login-info.dto';
import { UserLoginResource } from '../resources/user-login.resource';
import { UserLoginService } from '../services/user-login.service';

@Component({
  selector: 'user-login',
  templateUrl: 'login.component.html',
  styleUrls: ['./credentials-container.styles.css']
})

export class LoginComponent implements OnInit {
  model: LoginDto = new LoginDto();
  errors: string[] = [];

  private handledDomainErrors = [
    'User_InvalidCredentials'
  ];

  constructor(
    private configuration: Configuration,
    private router: Router,
    private resource: UserLoginResource,
    private userLoginService: UserLoginService,
    private loadingIndicator: LoadingIndicatorService
  ) { }

  ngOnInit() {
    if (localStorage.getItem(this.configuration.tokenProperty)) {
      this.router.navigate(['']);
    }
  }

  login() {
    this.loadingIndicator.show();
    this.resource.login(this.model)
      .subscribe(
        (userLoginInfoDto: UserLoginInfoDto) => {
          this.userLoginService.login(userLoginInfoDto);
          this.loadingIndicator.hide();
        },
        (err: any) => {
          if (err instanceof HttpErrorResponse) {
            this.errors = [];
            if (err.status === 422) {
              const data = err.error as DomainError;
              for (let i = 0; i <= data.messages.length - 1; i++) {
                const domainErrorCode = data.messages[i].domainErrorCode;
                if (this.handledDomainErrors.indexOf(domainErrorCode) >= 0) {
                  this.errors.push('user.login.errors.' + domainErrorCode);
                }
              }
            }
          }

          this.loadingIndicator.hide();
        }
      );
  }
}
