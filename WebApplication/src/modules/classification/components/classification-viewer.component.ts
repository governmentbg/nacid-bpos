import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { ClassificationDto } from '../dtos/classification.dto';
import { ClassificationBaseComponent } from './classification-base.component';

@Component({
  selector: 'classification-viewer',
  templateUrl: 'classification-viewer.component.html',
  styleUrls: ['./classification-viewer.styles.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})

export class ClassificationViewerComponent extends ClassificationBaseComponent {
  @Input() classification: ClassificationDto;

  @Input() collection: ClassificationDto[];
}
