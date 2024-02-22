import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { PublicationStatus } from '../enums/publication-status.enum';

@Component({
  selector: 'publication-status',
  templateUrl: 'publication-status.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})

export class PublicationStatusComponent {
  @Input() status: PublicationStatus;

  publicationStatuses = PublicationStatus;
}
