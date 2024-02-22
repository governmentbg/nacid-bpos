import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { Observable } from 'rxjs';
import { ConfirmDialogComponent } from 'src/infrastructure/components/confirm-modal/confirm-modal.component';
import { LoadingIndicatorService } from 'src/infrastructure/components/loading-indicator/loading-indicator.service';
import { SystemMessageType } from 'src/infrastructure/system-messages/enums/system-message-type.enum';
import { SystemMessage } from 'src/infrastructure/system-messages/models/system-message.model';
import { SystemMessagesHandlerService } from 'src/infrastructure/system-messages/services/system-messages-handler.service';
import { InstitutionResource } from '../institution.resource';
import { Institution } from '../models/institution.model';

@Component({
  selector: 'institution-edit',
  templateUrl: 'institution-edit.component.html'
})

export class InstitutionEditComponent implements OnInit {
  model: Institution = new Institution();

  isEditMode = false;
  originalObject = new Institution();

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private resource: InstitutionResource,
    private dialog: MatDialog,
    private translateService: TranslateService,
    private loadingIndicator: LoadingIndicatorService,
    private messagesService: SystemMessagesHandlerService
  ) { }

  ngOnInit() {
    this.route.params.subscribe((params: Params) => {
      const id = +params['id'];
      if (id) {
        this.loadingIndicator.show();
        this.resource.getById(id)
          .subscribe((institution: Institution) => {
            this.model = institution;
            this.loadingIndicator.hide();
          });
      } else if (params['id'] !== 'new') {
        this.router.navigate(['institutions']);
      } else {
        this.isEditMode = true;
      }
    })
  }

  save() {
    this.loadingIndicator.show();

    if (this.model.id) {
      this.resource.update(this.model.id, this.model)
        .subscribe((institution: Institution) => {
          this.model = institution;

          this.originalObject = new Institution();
          this.isEditMode = false;

          const message = new SystemMessage('institution.messages.modificationSuccessMessage', SystemMessageType.success);
          this.messagesService.messages.emit(message);

          this.loadingIndicator.hide();
        });
    } else {
      this.resource.post(this.model)
        .subscribe((institution: Institution) => {
          this.loadingIndicator.hide();

          const message = new SystemMessage('institution.messages.creationSuccessMessage', SystemMessageType.success);
          this.messagesService.messages.emit(message);

          this.router.navigate(['institutions', institution.id]);
        });
    }
  }

  edit() {
    if (this.isEditMode) {
      return;
    }

    this.originalObject = JSON.parse(JSON.stringify(this.model));
    this.isEditMode = true;
  }

  cancel() {
    if (!this.isEditMode) {
      return;
    }

    if (!this.model.id) {
      this.router.navigate(['institutions']);
    }

    this.model = JSON.parse(JSON.stringify(this.originalObject));
    this.originalObject = new Institution();
    this.isEditMode = false;
  }

  deactivate() {
    this.translateService.get([
      'institution.deactivationConfirmation.header',
      'institution.deactivationConfirmation.body',
      'institution.deactivationConfirmation.acceptText',
      'institution.deactivationConfirmation.declineText'
    ])
      .subscribe((translation: any) => {
        this.openDialog(
          translation['institution.deactivationConfirmation.header'],
          translation['institution.deactivationConfirmation.body'],
          translation['institution.deactivationConfirmation.acceptText'],
          translation['institution.deactivationConfirmation.declineText']
        )
          .subscribe((result: boolean) => {
            if (result) {
              this.saveDeactivation();
            }
          });
      });
  }

  private saveDeactivation(): void {
    this.loadingIndicator.show();
    this.resource.deactivate(this.model.id)
      .subscribe(() => {
        this.loadingIndicator.hide();

        const message = new SystemMessage('institution.messages.deactivationSuccessMessage', SystemMessageType.success);
        this.messagesService.messages.emit(message);

        this.router.navigate(['institutions']);
      })
  }

  private openDialog(header: string, body: string, acceptText: string, declineText: string): Observable<boolean> {
    return this.dialog.open(ConfirmDialogComponent, {
      disableClose: true,
      data: {
        header: header,
        body: body,
        acceptText: acceptText,
        showDecline: true,
        declineText: declineText
      }
    })
      .afterClosed()
  }
}
