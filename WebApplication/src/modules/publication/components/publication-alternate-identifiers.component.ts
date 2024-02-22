import { ChangeDetectionStrategy, Component, EventEmitter, Output, ViewChild } from '@angular/core';
import { PublicationAlternateIdentifier } from '../models/publication-alternate-identifier.model';
import { MultiplePublicationEntityBaseComponent } from './multiple-publication-entity-base.component';

@Component({
  selector: 'publication-alternate-identifiers',
  templateUrl: 'publication-alternate-identifiers.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})

export class PublicationAlternateIdentifiersComponent extends MultiplePublicationEntityBaseComponent<PublicationAlternateIdentifier> {
  @ViewChild('alternateIdentifiersForm', { static: false }) alternateIdentifiersForm;

  @Output() isValidForm: EventEmitter<boolean> = new EventEmitter();

  constructor() {
    super(PublicationAlternateIdentifier);
  }

  ngAfterViewInit() {
    if (!this.disabled) {
      this.alternateIdentifiersForm.statusChanges.subscribe(() => this.isValidForm.emit(this.alternateIdentifiersForm.valid));
    }
  }
}
