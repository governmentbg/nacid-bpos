import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { EMPTY, Observable, throwError as observableThrowError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { MenuItemService } from 'src/app/menu-items.service';
import { UserLoginService } from 'src/modules/user/services/user-login.service';
import { DomainError } from '../base/models/domain-error.model';
import { LoadingIndicatorService } from '../components/loading-indicator/loading-indicator.service';
import { SystemMessageType } from '../system-messages/enums/system-message-type.enum';
import { SystemMessage } from '../system-messages/models/system-message.model';
import { SystemMessagesHandlerService } from '../system-messages/services/system-messages-handler.service';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  private handledInComponent: string[] = [
    'User_EmailTaken',
    'User_ChangePasswordOldPasswordMismatch',
    'User_ChangePasswordNewPasswordMismatch'
  ];


  constructor(
    private router: Router,
    private userLoginService: UserLoginService,
    private menuItemsService: MenuItemService,
    private loadingIndicatorService: LoadingIndicatorService,
    private messageHandlerService: SystemMessagesHandlerService
  ) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    return next.handle(req).pipe(
      catchError((err: any) => {
        if (err instanceof HttpErrorResponse) {
          if (err.status === 401) {
            this.userLoginService.logout();
            this.router.navigate(['login']);

            this.loadingIndicatorService.hide();

            return EMPTY;
          } else if (err.status === 403) {
            const firstActive = this.menuItemsService.getFirstActive();
            this.router.navigate([firstActive.route]);

            this.loadingIndicatorService.hide();

            return EMPTY;
          } else if (err.status === 422) {
            const data = err.error as DomainError;
            for (const errorMessage of data.messages) {
              if (this.handledInComponent.indexOf(errorMessage.domainErrorCode) >= 0) {
                continue;
              }

              const errorCode = errorMessage.domainErrorCode as string;
              const errorKey = 'app.errorTexts.' + errorCode.substr(0, 1).toLowerCase() + errorCode.substr(1, errorCode.length);
              const message = new SystemMessage(errorKey, SystemMessageType.danger);
              this.messageHandlerService.messages.emit(message);

              this.loadingIndicatorService.hide();
            }
          }
        }

        return observableThrowError(err);
      }));
  }
}
