import { Component, OnInit } from '@angular/core';
import { RelationType } from '../models/relation-type.model';
import { BaseNomenclatureComponent } from './base-nomenclature.component';
@Component({
  selector: 'relation-types',
  templateUrl: 'foreign-name-nomenclature.component.html'
})

export class RelationTypesComponent extends BaseNomenclatureComponent<RelationType>
  implements OnInit {

  ngOnInit() {
    this.init(RelationType, 'RelationType');
  }

}
