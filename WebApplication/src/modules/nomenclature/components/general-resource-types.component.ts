import { Component, OnInit } from '@angular/core';
import { ResourceTypeGeneral } from '../models/resource-type-general.model';
import { BaseNomenclatureComponent } from './base-nomenclature.component';

@Component({
  selector: 'general-resource-types',
  templateUrl: 'foreign-name-nomenclature.component.html'
})

export class GeneralResourceTypesComponent extends BaseNomenclatureComponent<ResourceTypeGeneral>
  implements OnInit {

  ngOnInit() {
    this.init(ResourceTypeGeneral, 'ResourceTypeGeneral');
  }

}
