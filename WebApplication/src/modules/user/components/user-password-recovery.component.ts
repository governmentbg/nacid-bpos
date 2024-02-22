import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { DomainError } from 'src/infrastructure/base/models/domain-error.model';
import { LoadingIndicatorService } from 'src/infrastructure/components/loading-indicator/loading-indicator.service';
import { PASSWORD_REGEX } from 'src/infrastructure/constants/password-regex.constant';
import { ForgottenPasswordRecoveryDto } from '../dtos/forgotten-password-recovery.dto';
import { UserPasswordRecoveryResource } from '../resources/user-password-recovery.resource';

@Component({
  selector: 'user-password-recovery',
  templateUrl: 'user-password-recovery.component.html',
  styleUrls: ['./credentials-container.styles.css']
})

export class UserPasswordRecoveryComponent implements OnInit {
  model = new ForgottenPasswordRecoveryDto();

  isSuccess: boolean | null = null;
  errors: string[] = [];

  passwordRegex = PASSWORD_REGEX;

  @ViewChild('passwordRecoveryForm', { static: false }) passwordRecoveryForm: NgForm;

  private handledDomainErrors = [
    'User_PasswordRecoveryTokenAlreadyUsed',
    'User_PasswordRecoveryTokenExpired',
    'System_IncorrectParameters'
  ];

  constructor(
    private resource: UserPasswordRecoveryResource,
    private loadingIndicator: LoadingIndicatorService,
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    this.model.token = this.route.snapshot.queryParams.token;
  }

  changePassword() {
    this.loadingIndicator.show();
    this.resource.recoverPassword(this.model)
      .subscribe(
        () => {
          this.isSuccess = true;
          this.model = new ForgottenPasswordRecoveryDto();

          this.loadingIndicator.hide();
        },
        (err) => {
          this.isSuccess = false;

          this.passwordRecoveryForm.controls['newPassword'].setErrors(null);
          this.passwordRecoveryForm.controls['newPasswordAgain'].setErrors(null);

          if (err instanceof HttpErrorResponse) {
            if (err.status === 422) {
              const data = err.error as DomainError;
              for (let i = 0; i <= data.messages.length - 1; i++) {
                const domainErrorCode = data.messages[i].domainErrorCode;
                if (this.handledDomainErrors.indexOf(domainErrorCode) >= 0) {
                  this.errors.push(`user.passwordRecovery.errors.${domainErrorCode}`);
                } else if (domainErrorCode === 'User_ChangePasswordNewPasswordMismatch') {
                  this.passwordRecoveryForm.controls['newPassword'].setErrors({ 'passwordMismatch': true });
                  this.passwordRecoveryForm.controls['newPasswordAgain'].setErrors({ 'passwordMismatch': true });
                }
              }
            }
          }

          this.loadingIndicator.hide();
        });
  }
}
