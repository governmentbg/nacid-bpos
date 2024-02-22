import { EventEmitter, Injectable } from '@angular/core';

@Injectable()
export class LoadingIndicatorService {
  private loadingEmitter: EventEmitter<boolean>;

  constructor() { this.loadingEmitter = new EventEmitter<boolean>(); }

  subscribe(next: (value: boolean) => void) {
    return this.loadingEmitter.subscribe(next);
  }

  show() {
    return this.loadingEmitter.emit(true);
  }

  hide() {
    return this.loadingEmitter.emit(false);
  }
}