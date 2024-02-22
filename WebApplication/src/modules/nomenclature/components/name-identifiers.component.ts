import { Component, OnInit } from '@angular/core';
import { NameIdentifierScheme } from '../models/name-identifier-scheme.model';
import { BaseNomenclatureComponent } from './base-nomenclature.component';

@Component({
  selector: 'name-identifiers',
  templateUrl: 'name-identifiers.component.html'
})

export class NameIdentifiersComponent extends BaseNomenclatureComponent<NameIdentifierScheme> implements OnInit {
  ngOnInit() {
    this.init(NameIdentifierScheme, 'NameIdentifierScheme');
  }
}
