import { Component, OnInit, ElementRef, ViewChild, Input } from '@angular/core';
import { faTimes } from '@fortawesome/free-solid-svg-icons';
import { parseScrollEvent, ScrollType } from 'src/infrastructure/utils';

@Component({
  selector: 'app-drawer',
  templateUrl: './drawer.component.html',
  styleUrls: ['./drawer.component.scss']
})
export class DrawerComponent {
  isOpen = false;
  faTimes = faTimes;

  preventScroll(event) {
    event.preventDefault();
  }

  @ViewChild('drawer', { static: false }) drawer: ElementRef;

  limitScroll(event: WheelEvent) {
    var eventType = parseScrollEvent(event, this.drawer.nativeElement);
    if (eventType != ScrollType.middle)
      event.preventDefault();
  }
}
