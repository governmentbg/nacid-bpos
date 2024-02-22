import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Input } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { SystemMessagesHandlerService } from 'src/infrastructure/system-messages/services/system-messages-handler.service';
import { ClassificationResource } from '../classification.resource';
import { ClassificationDto } from '../dtos/classification.dto';
import { ClassificationBaseComponent } from './classification-base.component';

@Component({
  selector: 'classification-children',
  templateUrl: 'classification-children.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})

export class ClassificationChildrenComponent extends ClassificationBaseComponent {
  @Input() children: ClassificationDto[];
  @Input() level: number;

  constructor(
    protected resource: ClassificationResource,
    protected dialog: MatDialog,
    protected router: Router,
    protected systemMessageService: SystemMessagesHandlerService,
    protected cd: ChangeDetectorRef
  ) {
    super(resource, dialog, router, systemMessageService, cd);
  }

}
