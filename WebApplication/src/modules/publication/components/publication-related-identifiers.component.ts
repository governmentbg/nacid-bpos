import { ChangeDetectionStrategy, Component, EventEmitter, Output, ViewChild } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { PublicationRelatedIdentifier } from '../models/publication-related-identifier.model';
import { MultiplePublicationEntityBaseComponent } from './multiple-publication-entity-base.component';

@Component({
  selector: 'publication-related-identifiers',
  templateUrl: 'publication-related-identifiers.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})

export class PublicationRelatedIdentifiersComponent extends MultiplePublicationEntityBaseComponent<PublicationRelatedIdentifier> {
  @ViewChild('relatedIdentifiersForm', { static: false }) relatedIdentifiersForm;

  @Output() isValidForm: EventEmitter<boolean> = new EventEmitter();

  constructor(public translateService: TranslateService) {
    super(PublicationRelatedIdentifier);
  }

  ngAfterViewInit() {
    if (!this.disabled) {
      this.relatedIdentifiersForm.statusChanges.subscribe(() => this.isValidForm.emit(this.relatedIdentifiersForm.valid));
    }
  }
}
