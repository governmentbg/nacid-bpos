import { Component, Input } from "@angular/core";

@Component({
  selector: 'ab-collapsable-text',
  templateUrl: 'collapsable-text.component.html',
  styleUrls: ['collapsable-text.component.scss']
})
export class CollapsableTextComponent {
  @Input()
  text: string = '';

  isCollapsed = false;
}