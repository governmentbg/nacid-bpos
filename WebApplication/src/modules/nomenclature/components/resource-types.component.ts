import { Component, OnInit } from '@angular/core';
import { ResourceType } from '../models/resource-type.model';
import { BaseNomenclatureComponent } from './base-nomenclature.component';

@Component({
  selector: 'resource-types',
  templateUrl: 'foreign-name-nomenclature.component.html'
})

export class ResourceTypesComponent extends BaseNomenclatureComponent<ResourceType>
  implements OnInit {

  ngOnInit() {
    this.init(ResourceType, 'ResourceType');
  }

}
