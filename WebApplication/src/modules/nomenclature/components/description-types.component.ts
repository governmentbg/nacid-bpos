import { Component, OnInit } from '@angular/core';
import { DescriptionType } from '../models/description-type.model';
import { BaseNomenclatureComponent } from './base-nomenclature.component';
@Component({
  selector: 'description-types',
  templateUrl: 'base-alias-nomenclature.component.html'
})

export class DescriptionTypesComponent extends BaseNomenclatureComponent<DescriptionType>
  implements OnInit {

  ngOnInit() {
    this.init(DescriptionType, 'DescriptionType');
  }

}
