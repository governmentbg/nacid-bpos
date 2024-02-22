import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { LoadingIndicatorService } from 'src/infrastructure/components/loading-indicator/loading-indicator.service';
import { SystemMessageType } from 'src/infrastructure/system-messages/enums/system-message-type.enum';
import { SystemMessage } from 'src/infrastructure/system-messages/models/system-message.model';
import { SystemMessagesHandlerService } from 'src/infrastructure/system-messages/services/system-messages-handler.service';
import { ClassificationResource } from '../classification.resource';
import { ClassificationDto } from '../dtos/classification.dto';
import { Classification } from '../models/classification.model';
import { ClassificationBaseComponent } from './classification-base.component';

@Component({
  selector: 'app-classifications',
  templateUrl: 'classifications.component.html'
})

export class ClassificationsComponent extends ClassificationBaseComponent implements OnInit {
  roots: ClassificationDto[] = [];

  constructor(
    protected resource: ClassificationResource,
    protected dialog: MatDialog,
    protected router: Router,
    protected systemMessageService: SystemMessagesHandlerService,
    protected cd: ChangeDetectorRef,
    private loadingIndicator: LoadingIndicatorService
  ) {
    super(resource, dialog, router, systemMessageService, cd);
  }

  ngOnInit() {
    this.loadingIndicator.show();
    this.resource.getClassificationRoots()
      .subscribe((roots: ClassificationDto[]) => {
        this.roots = roots;
        this.loadingIndicator.hide();
      });
  }

  createClassification(classification: Classification, parent: ClassificationDto) {
    this.resource.createClassification(classification)
      .subscribe((classification: ClassificationDto) => {
        this.roots = [...this.roots, classification];

        const systemMessage = new SystemMessage('classification.base.successfullyAddedClassificationMsg', SystemMessageType.success);
        this.systemMessageService.messages.emit(systemMessage);

        this.cd.markForCheck();
      });
  }

  updateClassification(classification: Classification, collection: ClassificationDto[]) {
    this.resource.updateClassification(classification)
      .subscribe((classificationDto: ClassificationDto) => {
        const index = this.roots.findIndex(e => e.id === classificationDto.id);
        this.roots[index] = classificationDto;

        const systemMessage = new SystemMessage('classification.base.successfullyUpdatedClassificationMsg', SystemMessageType.success);
        this.systemMessageService.messages.emit(systemMessage);

        this.cd.markForCheck();
      });
  }
}
