import { AfterViewInit, ChangeDetectionStrategy, Component, EventEmitter, Output, ViewChild } from '@angular/core';
import { PublicationFundingReference } from '../models/publication-funding-reference.model';
import { MultiplePublicationEntityBaseComponent } from './multiple-publication-entity-base.component';

@Component({
  selector: 'publication-funding-references',
  templateUrl: 'publication-funding-references.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})

export class PublicationFundingReferencesComponent extends MultiplePublicationEntityBaseComponent<PublicationFundingReference> implements AfterViewInit {
  @ViewChild('fundingReferencesForm', { static: false }) fundingReferencesForm;

  @Output() isValidForm: EventEmitter<boolean> = new EventEmitter();

  constructor() {
    super(PublicationFundingReference);
  }

  ngAfterViewInit() {
    if (!this.disabled) {
      this.fundingReferencesForm.statusChanges.subscribe(() => this.isValidForm.emit(this.fundingReferencesForm.valid));
    }
  }
}
