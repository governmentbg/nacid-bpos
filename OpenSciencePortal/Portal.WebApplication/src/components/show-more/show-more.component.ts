import { Component, OnInit, Input, TemplateRef, ElementRef, ContentChild } from '@angular/core';

@Component({
  selector: 'app-show-more',
  templateUrl: './show-more.component.html',
  styleUrls: ['./show-more.component.scss']
})
export class ShowMoreComponent {
  @Input()
  collection: Array<any> = [];

  @Input()
  visibleItems: number = 4;

  showMore: boolean = false;

  @ContentChild('itemTemplate', { static: true })
  itemTemplate: TemplateRef<ElementRef>;

  show() {
    this.showMore = true;
  }

  hide() {
    this.showMore = false;
  }
}
