import { EventEmitter, Input, Output } from '@angular/core';
import { PublicationEntity } from '../models/publication-entity.model';

export class MultiplePublicationEntityBaseComponent<T extends PublicationEntity>  {
  collection: T[];

  @Input('collection') set collectionSetter(value: T[]) {
    this.collection = value || [];
  }
  @Output() collectionChange: EventEmitter<T[]> = new EventEmitter();

  @Input() isExpanded = true;

  @Input() disabled = true;

  private type: new () => T;

  constructor(type: new () => T) {
    this.type = type;
  }

  add() {
    const newItem = new this.type();
    newItem.viewOrder = this.collection && this.collection.length
      ? this.collection[this.collection.length - 1].viewOrder + 1
      : 1;

    this.collection.push(newItem);
    this.collectionChange.emit(this.collection);
  }

  remove(index: number) {
    this.collection.splice(index, 1);
    this.collectionChange.emit(this.collection);
  }
}
