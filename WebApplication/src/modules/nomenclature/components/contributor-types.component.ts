import { Component, OnInit } from '@angular/core';
import { ContributorType } from '../models/contributor-type.model';
import { BaseNomenclatureComponent } from './base-nomenclature.component';
@Component({
  selector: 'contributor-types',
  templateUrl: 'foreign-name-nomenclature.component.html'
})

export class ContributorTypesComponent extends BaseNomenclatureComponent<ContributorType>
  implements OnInit {

  ngOnInit() {
    this.init(ContributorType, 'ContributorType');
  }

}
