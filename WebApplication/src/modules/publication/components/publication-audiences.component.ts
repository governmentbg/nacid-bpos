import { ChangeDetectionStrategy, Component, EventEmitter, Output, ViewChild } from '@angular/core';
import { PublicationAudience } from '../models/publication-audience.model';
import { MultiplePublicationEntityBaseComponent } from './multiple-publication-entity-base.component';

@Component({
  selector: 'publication-audiences',
  templateUrl: 'publication-audiences.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})

export class PublicationAudiencesComponent extends MultiplePublicationEntityBaseComponent<PublicationAudience> {
  @ViewChild('audiencesForm', { static: false }) audiencesForm;

  @Output() isValidForm: EventEmitter<boolean> = new EventEmitter();

  constructor() {
    super(PublicationAudience);
  }

  ngAfterViewInit() {
    if (!this.disabled) {
      this.audiencesForm.statusChanges.subscribe(() => this.isValidForm.emit(this.audiencesForm.valid));
    }
  }
}
