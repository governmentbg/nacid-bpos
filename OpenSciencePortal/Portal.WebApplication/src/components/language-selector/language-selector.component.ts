import { Component } from "@angular/core";
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'ab-language-selector',
  templateUrl: 'language-selector.component.html',
  styleUrls: ['language-selector.component.scss']
})
export class LanguageSelectorComponent {
  constructor(
    private translateService: TranslateService
  ) { }

  languages = [
    { name: 'EN', language: 'en' },
    { name: 'БГ', language: 'bg' }
  ];

  selected = this.languages.find(language => language.language == this.translateService.currentLang);

  selectLanguage(language: { name: string, language: string }) {
    this.selected = language;
    this.translateService.use(language.language);
  }
}