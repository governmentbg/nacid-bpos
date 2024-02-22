import { Component, OnInit } from '@angular/core';
import { AccessRight } from '../models/access-right.model';
import { BaseNomenclatureComponent } from './base-nomenclature.component';

@Component({
  selector: 'access-rights',
  templateUrl: 'access-rights.component.html'
})

export class AccessRightsComponent extends BaseNomenclatureComponent<AccessRight> implements OnInit {
  ngOnInit() {
    this.init(AccessRight, 'AccessRight');
  }
}
