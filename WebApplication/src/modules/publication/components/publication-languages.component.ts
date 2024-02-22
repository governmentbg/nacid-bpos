import { ChangeDetectionStrategy, Component, EventEmitter, Output, ViewChild } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { PublicationLanguage } from '../models/publication-language.model';
import { MultiplePublicationEntityBaseComponent } from './multiple-publication-entity-base.component';

@Component({
  selector: 'publication-languages',
  templateUrl: 'publication-languages.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})

export class PublicationLanguagesComponent extends MultiplePublicationEntityBaseComponent<PublicationLanguage> {
  @ViewChild('languagesForm', { static: false }) languagesForm;

  @Output() isValidForm: EventEmitter<boolean> = new EventEmitter();

  constructor(public translateService: TranslateService) {
    super(PublicationLanguage);
  }

  ngAfterViewInit() {
    if (!this.disabled) {
      this.languagesForm.statusChanges.subscribe(() => this.isValidForm.emit(this.languagesForm.valid));
    }
  }
}
