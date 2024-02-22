import { Component, OnInit } from '@angular/core';
import { LicenseType } from '../models/license-type.model';
import { BaseNomenclatureComponent } from './base-nomenclature.component';

@Component({
  selector: 'license-types',
  templateUrl: 'license-types.component.html'
})

export class LicenseTypesComponent extends BaseNomenclatureComponent<LicenseType> implements OnInit {
  ngOnInit() {
    this.init(LicenseType, 'LicenseType');
  }
}
