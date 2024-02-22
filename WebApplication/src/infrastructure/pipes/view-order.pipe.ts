import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'vieworder' })
export class ViewOrderPipe implements PipeTransform {
  transform(value: any[], ascending = true): any[] {
    if (value) {
      const orderMultiplier = ascending ? 1 : -1;

      value.sort((a: any, b: any) => {
        if (a.viewOrder < b.viewOrder) {
          return -1 * orderMultiplier;
        } else if (a.viewOrder > b.viewOrder) {
          return 1 * orderMultiplier;
        } else { return 0; }
      });
    }

    return value;
  }
}
