import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { LanguageDto } from './dtos/language.dto';

@Component({
  selector: 'language-select',
  templateUrl: 'language-select.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})

export class LanguageSelectComponent implements OnInit {
  @Input() languages: LanguageDto[] = [];

  constructor(
    private translateService: TranslateService
  ) { }

  ngOnInit() {
    const index = this.languages.findIndex(e => e.alias === this.translateService.currentLang);
    if (index >= 0) {
      this.languages[index].isUsed = true;
    }
  }

  useLanguage(languageAlias: string) {
    this.translateService.use(languageAlias);
    this.languages.forEach(language => {
      if (language.alias !== languageAlias) {
        language.isUsed = false;
      } else {
        language.isUsed = true;
      }
    });
  }
}
