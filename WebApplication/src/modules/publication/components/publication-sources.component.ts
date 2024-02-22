import { AfterViewInit, ChangeDetectionStrategy, Component, EventEmitter, Output, ViewChild } from '@angular/core';
import { PublicationSource } from '../models/publication-source.model';
import { MultiplePublicationEntityBaseComponent } from './multiple-publication-entity-base.component';

@Component({
  selector: 'publication-sources',
  templateUrl: 'publication-sources.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})

export class PublicationSourcesComponent extends MultiplePublicationEntityBaseComponent<PublicationSource> implements AfterViewInit {
  @ViewChild('sourcesForm', { static: false }) sourcesForm;

  @Output() isValidForm: EventEmitter<boolean> = new EventEmitter();

  constructor() {
    super(PublicationSource);
  }

  ngAfterViewInit() {
    if (!this.disabled) {
      this.sourcesForm.statusChanges.subscribe(() => this.isValidForm.emit(this.sourcesForm.valid));
    }
  }
}
