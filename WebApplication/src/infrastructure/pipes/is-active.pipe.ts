import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'isActive' })
export class IsActivePipe implements PipeTransform {
  transform(collection: any[]) {
    return collection.filter(item => item.isActive);
  }
}
