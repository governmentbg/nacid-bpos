import { StepperSelectionEvent } from '@angular/cdk/stepper';
import { AfterViewInit, ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { Observable } from 'rxjs';
import { ConfirmDialogComponent } from 'src/infrastructure/components/confirm-modal/confirm-modal.component';
import { LoadingIndicatorService } from 'src/infrastructure/components/loading-indicator/loading-indicator.service';
import { UserPermissionVerificatorService } from 'src/infrastructure/services/user-permission-verification.service';
import { SystemMessageType } from 'src/infrastructure/system-messages/enums/system-message-type.enum';
import { SystemMessage } from 'src/infrastructure/system-messages/models/system-message.model';
import { SystemMessagesHandlerService } from 'src/infrastructure/system-messages/services/system-messages-handler.service';
import { FlatClassificationHierarchyItemDto } from 'src/modules/classification/dtos/flat-classification-hierarchy-item.dto';
import { InstitutionResource } from 'src/modules/institution/institution.resource';
import { Institution } from 'src/modules/institution/models/institution.model';
import { AccessRight } from 'src/modules/nomenclature/models/access-right.model';
import { NomenclatureResource } from 'src/modules/nomenclature/nomenclature.resource';
import { PublicationStep } from '../dtos/publication-step.dto';
import { PublicationStatus } from '../enums/publication-status.enum';
import { PublicationClassification } from '../models/publication-classification.model';
import { Publication } from '../models/publication.model';
import { PublicationResource } from '../publication.resource';

@Component({
  selector: 'publication',
  templateUrl: 'publication.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})

export class PublicationComponent implements OnInit, AfterViewInit {
  model: Publication = new Publication();
  isEditMode = false;
  arePublicationActionsVisible = false;
  canEditPublication = false;
  canMarkPendingApproval = false;
  canDeny = false;
  canPublish = false;
  private originalModel = new Publication();

  isValid = false;

  steps: PublicationStep[] = [
    new PublicationStep(false),
    new PublicationStep(true),
    new PublicationStep(false),
    new PublicationStep(false),
    new PublicationStep(false)
  ];

  hideEmptyPanels = false;

  publicationStatuses = PublicationStatus;

  accessRights: AccessRight[];
  hierarchyItems: FlatClassificationHierarchyItemDto[] = [];

  @ViewChild('resourceTypeForm', { static: false }) resourceTypeForm: NgForm;
  @ViewChild('publicationIdentifierForm', { static: false }) publicationIdentifierForm: NgForm;
  @ViewChild('publicationDateForm', { static: false }) publicationDateForm: NgForm;
  @ViewChild('citationsForm', { static: false }) citationsForm: NgForm;
  @ViewChild('rightsForm', { static: false }) rightsForm: NgForm;
  @ViewChild('licenseForm', { static: false }) licenseForm: NgForm;
  @ViewChild('classificationsForm', { static: false }) classificationsForm: NgForm;

  constructor(
    private resource: PublicationResource,
    private institutionResource: InstitutionResource,
    private accessRightsResource: NomenclatureResource<AccessRight>,
    private router: Router,
    private route: ActivatedRoute,
    private dialog: MatDialog,
    public translateService: TranslateService,
    private userPermissionVerificatorService: UserPermissionVerificatorService,
    private loadingIndicator: LoadingIndicatorService,
    private systemMessagesService: SystemMessagesHandlerService,
    private cd: ChangeDetectorRef
  ) {
    this.accessRightsResource.setSuffix('AccessRight');

    this.canPublish = this.userPermissionVerificatorService.hasFullAccess();
  }

  ngOnInit() {
    this.route.paramMap
      .subscribe((params: any) => {
        if (params.has('id')) {
          const id = +params.params.id;
          this.loadPublication(id);
        } else {
          this.isEditMode = true;
          this.arePublicationActionsVisible = false;

          const state = window.history.state;
          if (state && state.classificationId) {
            let newClassificaiton = new PublicationClassification();
            newClassificaiton.classificationId = state.classificationId;
            if (!this.model.classifications) {
              this.model.classifications = [];
            }
            this.model.classifications.push(newClassificaiton);
          }
        }
      });
  }

  ngAfterViewInit() {
    this.resourceTypeForm.statusChanges.subscribe(() => this.changeValidState(0, 'resourceTypeForm', this.resourceTypeForm.valid));
    this.publicationIdentifierForm.statusChanges.subscribe(() => this.changeValidState(0, 'publicationIdentifierForm', this.publicationIdentifierForm.valid));
    this.publicationDateForm.statusChanges.subscribe(() => this.changeValidState(0, 'publicationDateForm', this.publicationDateForm.valid));

    this.citationsForm.statusChanges.subscribe(() => this.changeValidState(1, 'citationsForm', this.citationsForm.valid));

    this.rightsForm.statusChanges.subscribe(() => this.changeValidState(2, 'rightsForm', this.rightsForm.valid));
    this.licenseForm.statusChanges.subscribe(() => this.changeValidState(2, 'licenseForm', this.licenseForm.valid));

    this.classificationsForm.statusChanges.subscribe(() => this.changeValidState(4, 'classificationsForm', this.classificationsForm.valid));
  }

  changeValidState(stepIndex: number, formName: string, isValid: boolean) {
    if (isValid === undefined || isValid === null) {
      return;
    }

    this.steps[stepIndex].changeFormValidState(formName, isValid);
    this.isValid = this.steps.findIndex(e => !e.isValid) < 0;
  }

  edit() {
    if (!this.model.id || this.isEditMode) {
      return;
    }

    this.originalModel = JSON.parse(JSON.stringify(this.model));
    this.isEditMode = true;
  }

  cancelEdit() {
    if (!this.model.id || !this.isEditMode) {
      return;
    }

    this.translateService.get([
      'publication.cancelConfirmationModal.header',
      'publication.cancelConfirmationModal.body',
      'publication.cancelConfirmationModal.acceptText',
      'publication.cancelConfirmationModal.declineText'
    ])
      .subscribe((translation: any) => {
        this.openDialog(
          translation['publication.cancelConfirmationModal.header'],
          translation['publication.cancelConfirmationModal.body'],
          translation['publication.cancelConfirmationModal.acceptText'],
          translation['publication.cancelConfirmationModal.declineText']
        )
          .subscribe((result: boolean) => {
            if (result) {
              this.model = JSON.parse(JSON.stringify(this.originalModel));
              this.originalModel = new Publication();
              this.isEditMode = false;
              this.cd.markForCheck();
            }
          });
      });
  }

  changeAccessRight(accessRightId: number) {
    this.model.accessRightId = accessRightId;
    this.model.accessRight = this.accessRights.filter(e => e.id === accessRightId)[0];
    this.model.embargoPeriodStart = null;
    this.model.embargoPeriodEnd = null;
  }

  selectionChanged(event: StepperSelectionEvent) {
    if (event.selectedIndex === 2) {
      if (!this.accessRights || !this.accessRights.length) {
        this.loadingIndicator.show();
        this.accessRightsResource.getFiltered()
          .subscribe((accessRights: AccessRight[]) => {
            this.accessRights = accessRights;
            this.loadingIndicator.hide();

            this.cd.markForCheck();
          });
      }
    } else if (event.selectedIndex === 4) {
      if (this.model.moderatorInstitutionId) {
        this.loadHierarchyItems(this.model.moderatorInstitutionId, this.model.classifications);
      }
    }
  }

  moderatorInstitutionChange(institution: Institution) {
    if (institution) {
      this.model.moderatorInstitutionId = institution.id;
      this.loadHierarchyItems(institution.id);
    } else {
      this.model.moderatorInstitutionId = null;
      this.hierarchyItems = [];
    }

    this.model.classifications = [];
  }

  private loadHierarchyItems(institutionId: number, publicationClassifications?: PublicationClassification[]) {
    this.loadingIndicator.show();
    this.institutionResource.getInstitutionClassifications(institutionId)
      .subscribe((hierarchyItems: FlatClassificationHierarchyItemDto[]) => {
        this.hierarchyItems = hierarchyItems;

        if (publicationClassifications) {
          publicationClassifications.forEach((classification: PublicationClassification) => {
            const index = this.hierarchyItems.findIndex(e => e.id === classification.classificationId);
            if (index >= 0) {
              this.hierarchyItems[index].isSelected = true;
            }
          })
        }

        this.loadingIndicator.hide();

        this.cd.markForCheck();
      });
  }

  changedItem(item: FlatClassificationHierarchyItemDto) {
    if (!item.isSelected) {
      const index = this.model.classifications.findIndex(e => e.classificationId === item.id);
      this.model.classifications.splice(index, 1);
    } else {
      const newClassification = new PublicationClassification();
      newClassification.classificationId = item.id;
      if (!this.model.classifications) {
        this.model.classifications = [];
      }
      this.model.classifications.push(newClassification);
    }
  }

  savePublication(publicationStatus?: PublicationStatus) {
    let bodyPath = 'publication.confirmationModal.'
    if (publicationStatus) {
      this.model.status = publicationStatus;
      bodyPath += `${PublicationStatus[this.model.status]}.body`;
    } else {
      bodyPath += 'defaultBody';
    }

    this.getTranslations(bodyPath)
      .subscribe((translation: any) => {
        this.openDialog(
          translation['publication.confirmationModal.header'],
          translation[bodyPath],
          translation['publication.confirmationModal.acceptText'],
          translation['publication.confirmationModal.declineText']
        )
          .subscribe((result: boolean) => {
            if (result) {
              this.save();
            }
          });
      });
  }

  markPending() {
    this.doAction('publication.confirmationModal.pendingApproval.body', this.resource.markPending(this.model.id), 'publication.messages.successMarkPendingMessage');
  }

  deny() {
    this.doAction('publication.confirmationModal.notApproved.body', this.resource.deny(this.model.id), 'publication.messages.successDenyMessage');
  }

  publish() {
    this.doAction('publication.confirmationModal.published.body', this.resource.publish(this.model.id), 'publication.messages.successPublishMessage');
  }

  private loadPublication(id: number) {
    this.loadingIndicator.show();
    this.resource.getById(id)
      .subscribe((model: Publication) => {
        this.model = model;
        this.isEditMode = false;

        this.arePublicationActionsVisible = (this.userPermissionVerificatorService.hasFullAccess() && model.status !== PublicationStatus.published && model.status !== PublicationStatus.deleted)
          || (this.userPermissionVerificatorService.isFromPublicationInstitution(model.moderatorInstitutionId) && (model.status === PublicationStatus.draft || model.status === PublicationStatus.notApproved || model.status === PublicationStatus.pendingApproval))
          || (this.userPermissionVerificatorService.isCreator(model.createdByUserId) && (model.status === PublicationStatus.draft || model.status === PublicationStatus.notApproved));

        this.canEditPublication = (this.userPermissionVerificatorService.hasFullAccess() && model.status !== PublicationStatus.published && model.status !== PublicationStatus.deleted)
          || (this.userPermissionVerificatorService.isFromPublicationInstitution(model.moderatorInstitutionId) && (model.status === PublicationStatus.draft || model.status === PublicationStatus.notApproved || model.status === PublicationStatus.pendingApproval))
          || (this.userPermissionVerificatorService.isCreator(model.createdByUserId) && (model.status === PublicationStatus.draft || model.status === PublicationStatus.notApproved));

        this.canMarkPendingApproval = this.userPermissionVerificatorService.isCreator(model.createdByUserId) && model.status === PublicationStatus.draft;

        this.canDeny = this.model.status === PublicationStatus.pendingApproval &&
          (this.userPermissionVerificatorService.hasFullAccess() || this.userPermissionVerificatorService.isFromPublicationInstitution(model.moderatorInstitutionId));

        this.canPublish = (this.userPermissionVerificatorService.hasFullAccess() && (model.status === PublicationStatus.draft || model.status === PublicationStatus.pendingApproval))
          || (this.model.status === PublicationStatus.pendingApproval && this.userPermissionVerificatorService.isFromPublicationInstitution(model.moderatorInstitutionId));

        this.steps[3].isValid = this.model.files && this.model.files.length > 0;

        this.cd.markForCheck();

        this.loadingIndicator.hide();
      });
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

  private getTranslations(bodyPath: string): Observable<any> {
    return this.translateService.get([
      'publication.confirmationModal.header',
      bodyPath,
      'publication.confirmationModal.acceptText',
      'publication.confirmationModal.declineText'
    ])
  }

  private save() {
    if (!this.model.id) {
      this.loadingIndicator.show();
      this.resource.post(this.model)
        .subscribe(
          (publication: Publication) => {
            const successMessage = new SystemMessage('publication.messages.successCreationMessage', SystemMessageType.success);
            this.systemMessagesService.messages.emit(successMessage);
            this.loadingIndicator.hide();
            this.router.navigate(['publications', publication.id, 'edit']);
          },
          () => {
            const errorMessage = new SystemMessage('publication.messages.failureCreationMessage', SystemMessageType.danger);
            this.systemMessagesService.messages.emit(errorMessage);
            this.loadingIndicator.hide();
          });

    } else {
      this.loadingIndicator.show();
      this.resource.update(this.model.id, this.model)
        .subscribe(() => {
          const message = new SystemMessage('publication.messages.successUpdateMessage', SystemMessageType.success);
          this.systemMessagesService.messages.emit(message);
          this.router.navigate(['publications']);
          this.loadingIndicator.hide();
        });
    }
  }

  private doAction(confirmationBody: string, action: Observable<void>, successMessageBody: string) {
    this.getTranslations(confirmationBody)
      .subscribe((translation: any) => {
        this.openDialog(
          translation['publication.confirmationModal.header'],
          translation[confirmationBody],
          translation['publication.confirmationModal.acceptText'],
          translation['publication.confirmationModal.declineText']
        )
          .subscribe((result: boolean) => {
            if (result) {
              this.loadingIndicator.show();
              action.subscribe(() => this.completeActionSuccessfully(successMessageBody))
            }
          });
      });
  }

  private completeActionSuccessfully(successMessageBody: string) {
    this.loadingIndicator.hide();

    const systemMessage = new SystemMessage(successMessageBody, SystemMessageType.success);
    this.systemMessagesService.messages.emit(systemMessage);

    this.router.navigate(['publications']);
  }
}
