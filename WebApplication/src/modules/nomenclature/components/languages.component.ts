import { Component, OnInit } from '@angular/core';
import { Language } from '../models/language.model';
import { BaseNomenclatureComponent } from './base-nomenclature.component';
@Component({
  selector: 'languages',
  templateUrl: 'foreign-name-nomenclature.component.html'
})

export class LanguagesComponent extends BaseNomenclatureComponent<Language>
  implements OnInit {

  ngOnInit() {
    this.init(Language, 'Language');
  }

}
