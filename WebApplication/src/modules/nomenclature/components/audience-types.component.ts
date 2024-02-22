import { Component, OnInit } from '@angular/core';
import { AudienceType } from '../models/audience-type.model';
import { BaseNomenclatureComponent } from './base-nomenclature.component';
@Component({
  selector: 'relation-types',
  templateUrl: 'foreign-name-nomenclature.component.html'
})

export class AudienceTypesComponent extends BaseNomenclatureComponent<AudienceType>
  implements OnInit {

  ngOnInit() {
    this.init(AudienceType, 'AudienceType');
  }

}
