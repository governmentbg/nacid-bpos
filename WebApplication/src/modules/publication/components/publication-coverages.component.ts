import { AfterViewInit, ChangeDetectionStrategy, Component, EventEmitter, Output, ViewChild } from '@angular/core';
import { PublicationCoverage } from '../models/publication-coverage.model';
import { MultiplePublicationEntityBaseComponent } from './multiple-publication-entity-base.component';

@Component({
  selector: 'publication-coverages',
  templateUrl: 'publication-coverages.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})

export class PublicationCoveragesComponent extends MultiplePublicationEntityBaseComponent<PublicationCoverage> implements AfterViewInit {
  @ViewChild('coveragesForm', { static: false }) coveragesForm;

  @Output() isValidForm: EventEmitter<boolean> = new EventEmitter();

  constructor() {
    super(PublicationCoverage);
  }

  ngAfterViewInit() {
    if (!this.disabled) {
      this.coveragesForm.statusChanges.subscribe(() => this.isValidForm.emit(this.coveragesForm.valid));
    }
  }
}
