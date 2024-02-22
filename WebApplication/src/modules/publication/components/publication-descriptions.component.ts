import { AfterViewInit, ChangeDetectionStrategy, Component, EventEmitter, Output, ViewChild } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { PublicationDescription } from '../models/publication-description.model';
import { MultiplePublicationEntityBaseComponent } from './multiple-publication-entity-base.component';

@Component({
  selector: 'publication-descriptions',
  templateUrl: 'publication-descriptions.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})

export class PublicationDescriptionsComponent extends MultiplePublicationEntityBaseComponent<PublicationDescription> implements AfterViewInit {
  @ViewChild('descriptionsForm', { static: false }) descriptionsForm;

  @Output() isValidForm: EventEmitter<boolean> = new EventEmitter();

  constructor(public translateService: TranslateService) {
    super(PublicationDescription);
  }

  ngAfterViewInit() {
    if (!this.disabled) {
      this.descriptionsForm.statusChanges.subscribe(() => this.isValidForm.emit(this.descriptionsForm.valid));
    }
  }

}
