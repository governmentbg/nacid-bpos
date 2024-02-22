import { Injectable, EventEmitter } from "@angular/core";
import { ReplaySubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoadingIndicatorService {
  //change$: EventEmitter<boolean> = new EventEmitter<boolean>();
  change$: ReplaySubject<boolean> = new ReplaySubject<boolean>();

  start() {
    this.change$.next(true);
  }

  stop() {
    this.change$.next(false);
  }
}