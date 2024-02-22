import { ChangeDetectionStrategy, Component } from '@angular/core';
import { PublicationFormat } from '../models/publication-format.model';
import { MultiplePublicationEntityBaseComponent } from './multiple-publication-entity-base.component';

@Component({
  selector: 'publication-formats',
  templateUrl: 'publication-formats.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})

export class PublicationFormatsComponent extends MultiplePublicationEntityBaseComponent<PublicationFormat> {

  constructor() {
    super(PublicationFormat);
  }

}
