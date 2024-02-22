import { HttpErrorResponse } from '@angular/common/http';
import { Component, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { DomainError } from 'src/infrastructure/base/models/domain-error.model';
import { LoadingIndicatorService } from 'src/infrastructure/components/loading-indicator/loading-indicator.service';
import { ForgottenPasswordDto } from '../dtos/forgotten-password.dto';
import { UserPasswordRecoveryResource } from '../resources/user-password-recovery.resource';

@Component({
  selector: 'user-forgotten-password',
  templateUrl: 'user-forgotten-password.component.html',
  styleUrls: ['./credentials-container.styles.css']
})

export class UserForgottenPasswordComponent {
  @ViewChild('forgottenPasswordForm', { static: false }) forgottenPasswordForm: NgForm;

  model = new ForgottenPasswordDto();

  isSuccess: boolean | null = null;

  private handledDomainErrors = [
    'User_InvalidCredentials',
    'User_CannotRestoreUserPassword'
  ];

  constructor(
    private resource: UserPasswordRecoveryResource,
    private loadingIndicator: LoadingIndicatorService
  ) { }

  sendForgottenPassword() {
    this.loadingIndicator.show();
    this.resource.sendForgottenPassword(this.model)
      .subscribe(
        () => {
          this.isSuccess = true;
          this.model = new ForgottenPasswordDto();

          this.loadingIndicator.hide();
        },
        (err) => {
          this.isSuccess = false;

          if (err instanceof HttpErrorResponse) {
            if (err.status === 422) {
              const data = err.error as DomainError;
              for (let i = 0; i <= data.messages.length - 1; i++) {
                const domainErrorCode = data.messages[i].domainErrorCode;
                if (this.handledDomainErrors.indexOf(domainErrorCode) >= 0) {
                  this.forgottenPasswordForm.controls['mail'].setErrors({ [`${domainErrorCode}`]: true });
                }
              }
            }
          }

          this.loadingIndicator.hide();
        });
  }
}
