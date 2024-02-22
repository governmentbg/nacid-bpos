import { Component, OnInit } from '@angular/core';
import { TitleType } from '../models/title-type.model';
import { BaseNomenclatureComponent } from './base-nomenclature.component';
@Component({
  selector: 'title-types',
  templateUrl: 'foreign-name-nomenclature.component.html'
})

export class TitleTypesComponent extends BaseNomenclatureComponent<TitleType>
  implements OnInit {

  ngOnInit() {
    this.init(TitleType, 'TitleType');
  }

}
