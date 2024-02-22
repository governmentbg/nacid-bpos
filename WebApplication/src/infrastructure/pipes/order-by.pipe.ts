import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'orderBy'
})
export class OrderByPipe implements PipeTransform {
  transform(array: any[], prop: string): any[] {
    if (!array) {
      return array;
    }

    let sortDirectionMultiplier = 1;
    if (prop.startsWith('-')) {
      prop = prop.slice(1);
      sortDirectionMultiplier = -1;
    }

    array.sort((a: any, b: any) => {
      const left = this.getValue(a, prop);
      const right = this.getValue(b, prop);
      if (left < right) {
        return (-1) * sortDirectionMultiplier;
      } else if (left > right) {
        return sortDirectionMultiplier;
      } else {
        return 0;
      }
    });

    return array;
  }

  private getValue(item: any, key: string): any {
    const innerProperties = key.split('.');
    let filteredProperty = innerProperties.length ? item : item[key];
    innerProperties.forEach(property => filteredProperty = filteredProperty[property]);

    return filteredProperty;
  }
}
