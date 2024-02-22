import { ChangeDetectionStrategy, Component } from '@angular/core';
import { PublicationSize } from '../models/publication-size.model';
import { MultiplePublicationEntityBaseComponent } from './multiple-publication-entity-base.component';

@Component({
  selector: 'publication-sizes',
  templateUrl: 'publication-sizes.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})

export class PublicationSizesComponent extends MultiplePublicationEntityBaseComponent<PublicationSize> {

  constructor() {
    super(PublicationSize);
  }

}
