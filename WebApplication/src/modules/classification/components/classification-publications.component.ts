import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { ClassificationPublicationDto } from '../dtos/classification-publication.dto';

@Component({
  selector: 'classification-publications',
  templateUrl: 'classification-publications.component.html',
  styleUrls: ['./classification-publications.styles.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})

export class ClassificationPublicationsComponent {
  @Input() publications: ClassificationPublicationDto[];
}
