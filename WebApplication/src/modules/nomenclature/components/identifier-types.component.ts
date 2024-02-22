import { Component, OnInit } from '@angular/core';
import { ResourceIdentifierType } from '../models/resource-identifier-type.model';
import { BaseNomenclatureComponent } from './base-nomenclature.component';

@Component({
  selector: 'resource-identifier-types',
  templateUrl: 'base-alias-nomenclature.component.html'
})

export class ResourceIdentifierTypesComponent extends BaseNomenclatureComponent<ResourceIdentifierType> implements OnInit {
  ngOnInit() {
    this.init(ResourceIdentifierType, 'ResourceIdentifierType');
  }
}
