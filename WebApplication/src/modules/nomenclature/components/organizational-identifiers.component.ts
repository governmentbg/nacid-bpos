import { Component, OnInit } from '@angular/core';
import { OrganizationalIdentifierScheme } from '../models/organizational-identifier-scheme.model';
import { BaseNomenclatureComponent } from './base-nomenclature.component';

@Component({
  selector: 'organizational-identifiers',
  templateUrl: 'organizational-identifiers.component.html'
})

export class OrganizationalIdentifiersComponent extends BaseNomenclatureComponent<OrganizationalIdentifierScheme> implements OnInit {
  ngOnInit() {
    this.init(OrganizationalIdentifierScheme, 'OrganizationalIdentifierScheme');
  }
}
