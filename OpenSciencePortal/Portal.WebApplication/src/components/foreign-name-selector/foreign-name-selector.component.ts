import { Component, Input, ContentChild, TemplateRef, ElementRef, ChangeDetectorRef, OnInit } from "@angular/core";
import { TranslateService, LangChangeEvent } from '@ngx-translate/core';

@Component({
  selector: 'foreign-name-select',
  templateUrl: 'foreign-name-selector.component.html'
})
export class ForeignNameSelectorComponent<T extends { name: string, nameEn: string }> implements OnInit {
  @Input('model')
  model: T;
  value = "";

  constructor(
    private translate: TranslateService,
    private changeDetector: ChangeDetectorRef
  ) {
  }

  ngOnInit() {
    this.value = this.translate.currentLang === "en" ? this.model.nameEn : this.model.name;

    this.translate.onLangChange.subscribe((event: LangChangeEvent) => {
      this.value = event.lang === "en" ? this.model.nameEn : this.model.name
      this.changeDetector.detectChanges();
    });
  }
}