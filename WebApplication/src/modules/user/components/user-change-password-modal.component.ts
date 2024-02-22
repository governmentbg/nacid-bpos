import { Component, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { DomainError } from 'src/infrastructure/base/models/domain-error.model';
import { LoadingIndicatorService } from 'src/infrastructure/components/loading-indicator/loading-indicator.service';
import { PASSWORD_REGEX } from 'src/infrastructure/constants/password-regex.constant';
import { SystemMessageType } from 'src/infrastructure/system-messages/enums/system-message-type.enum';
import { SystemMessage } from 'src/infrastructure/system-messages/models/system-message.model';
import { SystemMessagesHandlerService } from 'src/infrastructure/system-messages/services/system-messages-handler.service';
import { UserChangePasswordDto } from '../dtos/user-change-password.dto';
import { UserResource } from '../resources/user.resource';

@Component({
  selector: 'user-change-password-modal',
  templateUrl: 'user-change-password-modal.component.html',
  styleUrls: ['./user-change-password-modal.styles.css']
})

export class UserChangePasswordModalComponent {
  @ViewChild('userChangePasswordForm', { static: false }) userChangePasswordForm: NgForm;

  model: UserChangePasswordDto = new UserChangePasswordDto();

  passwordRegex = PASSWORD_REGEX;

  constructor(
    private dialogRef: MatDialogRef<UserChangePasswordModalComponent>,
    private resource: UserResource,
    private systemMessagesService: SystemMessagesHandlerService,
    private loadingIndicator: LoadingIndicatorService
  ) { }

  change() {
    this.loadingIndicator.show();

    this.resource.changePassword(this.model)
      .subscribe(() => {
        const message = new SystemMessage('user.changePasswordModal.successMessage', SystemMessageType.success);
        this.systemMessagesService.messages.emit(message);

        this.loadingIndicator.hide();

        this.close();
      },
        (err: any) => {
          this.userChangePasswordForm.controls['oldPassword'].setErrors(null);
          this.userChangePasswordForm.controls['newPassword'].setErrors(null);
          this.userChangePasswordForm.controls['newPasswordAgain'].setErrors(null);

          if (err.status === 422) {
            const data = err.error as DomainError;
            for (let i = 0; i <= data.messages.length - 1; i++) {
              const domainErrorCode = data.messages[i].domainErrorCode;
              if (domainErrorCode === 'User_ChangePasswordOldPasswordMismatch') {
                this.userChangePasswordForm.controls['oldPassword'].setErrors({ 'incorrectPassword': true });
              } else if (domainErrorCode === 'User_ChangePasswordNewPasswordMismatch') {
                this.userChangePasswordForm.controls['newPassword'].setErrors({ 'passwordMismatch': true });
                this.userChangePasswordForm.controls['newPasswordAgain'].setErrors({ 'passwordMismatch': true });
              }
            }
          }

          this.loadingIndicator.hide();
        });
  }

  close() {
    this.dialogRef.close();
  }
}
