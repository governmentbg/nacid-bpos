import { Component, Input, ViewChild, OnInit, ContentChild, TemplateRef, ElementRef } from "@angular/core";
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { faAngleDown } from '@fortawesome/free-solid-svg-icons';
import { NgbDropdown } from '@ng-bootstrap/ng-bootstrap';
import { parseScrollEvent, ScrollType } from 'src/infrastructure/utils';

/**
 * @usage
 * <ab-select [collection]="collection"
                formControlName="<control-name>">
      <ng-template #dropdownItemTemplate
                    let-item>
        {{item}}
      </ng-template>
      <ng-template #selectedItemTemplate
                    let-value>
        {{value}}
      </ng-template>
    </ab-select>
 */
@Component({
  selector: 'ab-select',
  templateUrl: 'select.component.html',
  styleUrls: ['select.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: SelectComponent,
      multi: true
    }
  ]
})
export class SelectComponent implements ControlValueAccessor, OnInit {
  @Input()
  collection: any[] = [];

  @ContentChild('dropdownItemTemplate', { static: true })
  dropdownItemTemplate: TemplateRef<ElementRef>;

  @ContentChild('selectedItemTemplate', { static: true })
  selectedItemTemplate: TemplateRef<ElementRef>;

  @ViewChild('dropdown', { static: true })
  dropdown: NgbDropdown;

  @ViewChild('dropdownMenu', { static: true })
  dropdownMenu: ElementRef;

  value: any;
  isOpen: boolean;
  formControlChangeHandler: any;
  faAngleDown = faAngleDown;

  ngOnInit() {
    this.dropdown.openChange.subscribe((isOpen: boolean) => this.isOpen = isOpen);
  }

  writeValue(obj: any): void {
    this.value = obj;
  }

  registerOnChange(fn: any): void {
    this.formControlChangeHandler = fn;
  }

  registerOnTouched(fn: any): void {
  }

  selectItem(item: any) {
    this.value = item;
    this.formControlChangeHandler(this.value);
  }

  limitScroll(event: WheelEvent) {
    var eventType = parseScrollEvent(event, this.dropdownMenu.nativeElement);
    if (eventType != ScrollType.middle)
      event.preventDefault();
  }
}