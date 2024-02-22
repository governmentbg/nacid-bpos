import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

export enum PropertyType {
  plain = 1,
  object = 2,
  array = 3
}

@Component({
  selector: 'app-publication-detail',
  templateUrl: './publication-detail.component.html',
  styleUrls: ['./publication-detail.component.scss']
})
export class PublicationDetailComponent {
  constructor(
    private route: ActivatedRoute
  ) {
    this.route.data.subscribe(({ publication }) => {
      this.publication = publication;
      this.processedPublication = formatObject(this.publication, 0);
    });
  }
  PropertyType = PropertyType;
  publication: any;
  processedPublication: any;
}

function formatObject(item: any, level: number) {
  return Object.keys(item).map(key => {
    if (Array.isArray(item[key])) {
      return {
        name: key,
        value: item[key].map(innerItem => formatObject(innerItem, level + 1)),
        __propertyType: PropertyType.array
      };
    } else if (item[key] instanceof Object) {
      return {
        name: key,
        value: formatObject(item[key], level + 1),
        __propertyType: PropertyType.object
      };
    }
    else
      return {
        name: key,
        value: item[key],
        __propertyType: PropertyType.plain
      }
  }).filter(item => item.value);
}
