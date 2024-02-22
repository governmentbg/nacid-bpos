import { AfterViewInit, ChangeDetectionStrategy, Component, EventEmitter, Output, ViewChild } from '@angular/core';
import { PublicationPublisher } from '../models/publication-publisher.model';
import { MultiplePublicationEntityBaseComponent } from './multiple-publication-entity-base.component';

@Component({
  selector: 'publication-publishers',
  templateUrl: 'publication-publishers.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})

export class PublicationPublishersComponent extends MultiplePublicationEntityBaseComponent<PublicationPublisher> implements AfterViewInit {
  @ViewChild('publishersForm', { static: false }) publishersForm;

  @Output() isValidForm: EventEmitter<boolean> = new EventEmitter();

  constructor() {
    super(PublicationPublisher);
  }

  ngAfterViewInit() {
    if (!this.disabled) {
      this.publishersForm.statusChanges.subscribe(() => this.isValidForm.emit(this.publishersForm.valid));
    }
  }
}
