import { AfterViewInit, ChangeDetectionStrategy, Component, EventEmitter, Output, ViewChild } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { PublicationTitle } from '../models/publication-title.model';
import { MultiplePublicationEntityBaseComponent } from './multiple-publication-entity-base.component';

@Component({
  selector: 'publication-titles',
  templateUrl: 'publication-titles.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})

export class PublicationTitlesComponent extends MultiplePublicationEntityBaseComponent<PublicationTitle> implements AfterViewInit {
  @ViewChild('titlesForm', { static: false }) titlesForm;

  @Output() isValidForm: EventEmitter<boolean> = new EventEmitter();

  constructor(public translateService: TranslateService) {
    super(PublicationTitle);
  }

  ngAfterViewInit() {
    if (!this.disabled) {
      this.titlesForm.statusChanges.subscribe(() => this.isValidForm.emit(this.titlesForm.valid));
    }
  }
}
