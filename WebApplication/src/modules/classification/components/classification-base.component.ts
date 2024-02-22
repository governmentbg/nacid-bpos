import { ChangeDetectorRef } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { SystemMessageType } from 'src/infrastructure/system-messages/enums/system-message-type.enum';
import { SystemMessage } from 'src/infrastructure/system-messages/models/system-message.model';
import { SystemMessagesHandlerService } from 'src/infrastructure/system-messages/services/system-messages-handler.service';
import { ClassificationResource } from '../classification.resource';
import { ClassificationDto } from '../dtos/classification.dto';
import { Classification } from '../models/classification.model';
import { ClassificationCreationModal } from './classification-creation-modal.component';

export abstract class ClassificationBaseComponent {
  constructor(
    protected resource: ClassificationResource,
    protected dialog: MatDialog,
    protected router: Router,
    protected systemMessageService: SystemMessagesHandlerService,
    protected cd: ChangeDetectorRef
  ) { }

  collapseChildren(classification: ClassificationDto) {
    classification.isExpanded = !classification.isExpanded;
    if (classification.isExpanded && !classification.children.length) {
      classification.isLoading = true;
      this.resource.getChildClassifications(classification.id)
        .subscribe((children: ClassificationDto[]) => {
          classification.children = children;
          classification.isExpanded = true;
          classification.isLoading = false;
          this.cd.markForCheck();
        });
    }
  }

  addClassification(parent: ClassificationDto, event: MouseEvent) {
    event.stopPropagation();

    this.dialog.open(ClassificationCreationModal, {
      width: '40%',
      disableClose: true,
      data: {
        parentClassification: parent
      }
    })
      .afterClosed()
      .subscribe((classification: Classification) => {
        if (classification) {
          this.createClassification(classification, parent);
        }
      });
  }

  addPublication(classificationId: number, event: MouseEvent) {
    event.stopPropagation();

    this.router.navigate(['publications'], { state: { classificationId: classificationId } });
  }

  editClassification(id: number, collection: ClassificationDto[]) {
    this.dialog.open(ClassificationCreationModal, {
      width: '40%',
      disableClose: true,
      data: {
        id: id
      }
    })
      .afterClosed()
      .subscribe((classification: Classification) => {
        if (classification) {
          this.updateClassification(classification, collection);
        }
      });
  }

  createClassification(classification: Classification, parent: ClassificationDto) {
    this.resource.createClassification(classification)
      .subscribe((classificationDto: ClassificationDto) => {
        parent.children = [...parent.children, classificationDto];

        const systemMessage = new SystemMessage('classification.base.successfullyAddedClassificationMsg', SystemMessageType.success);
        this.systemMessageService.messages.emit(systemMessage);

        this.cd.markForCheck();
      });
  }

  updateClassification(classification: Classification, collection: ClassificationDto[]) {
    this.resource.updateClassification(classification)
      .subscribe((classificationDto: ClassificationDto) => {
        const index = collection.findIndex(e => e.id === classificationDto.id);
        collection[index] = classificationDto;

        const systemMessage = new SystemMessage('classification.base.successfullyUpdatedClassificationMsg', SystemMessageType.success);
        this.systemMessageService.messages.emit(systemMessage);

        this.cd.markForCheck();
      });
  }

  deleteClassification(id: number, collection: ClassificationDto[]) {
    this.resource.deleteClassification(id)
      .subscribe(() => {
        const index = collection.findIndex(e => e.id === id);
        collection.splice(index, 1);

        const systemMessage = new SystemMessage('classification.base.successfullyRemovedClassificationMsg', SystemMessageType.success);
        this.systemMessageService.messages.emit(systemMessage);

        this.cd.markForCheck();
      })
  }
}
