import { HttpClient } from '@angular/common/http';
// tslint:disable-next-line: max-line-length
import { ChangeDetectorRef, Component, ElementRef, forwardRef, HostBinding, HostListener, Input, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { Observable, Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { Entity } from 'src/infrastructure/base/models/entity.model';
import { NomenclatureSelectFilter } from '../dtos/nomenclature-select-filter.dto';
import { AbNomenclatureTemplatePipe } from './ab-nomenclature-template.pipe';

@Component({
  selector: 'ab-nomenclature-select',
  templateUrl: 'ab-nomenclature-select.component.html',
  styleUrls: ['ab-nomenclature-select.styles.css'],
  encapsulation: ViewEncapsulation.None,
  providers: [
    AbNomenclatureTemplatePipe,
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => AbNomenclatureSelectComponent),
      multi: true
    }
  ]
})
export class AbNomenclatureSelectComponent<T extends Entity> implements ControlValueAccessor, OnInit {
  @Input('filter')
  set filterSetter(filter: any) {
    this.filter = filter;
    this.options = [];
  }

  @HostBinding('class.disabled-nomenclature-select') get disabledFlag() {
    return this.disabled;
  }

  selectedModel: T | null;
  textFilter: string | null;
  @Input() options: T[] = [];

  selectOpened = false;

  // @Input() collection: any[];
  // @Input() filterCollectionBy: string;

  filter: any = new NomenclatureSelectFilter();
  @Input() restUrl: string;
  @Input() keyProperty = 'id';
  @Input() limit = 10;

  @Input() hint: string;

  isLoading = false;
  allLoaded = false;

  @Input() allowClear = true;
  @Input() textTemplate: string;

  @Input() disabled = false;
  @Input() required = false;

  @Input() placeholder: string;

  private textFilterChanged = new Subject<string>();

  constructor(
    private changeDetectorRef: ChangeDetectorRef,
    private httpClient: HttpClient,
    private elementRef: ElementRef,
    private templatePipe: AbNomenclatureTemplatePipe
  ) { }

  @HostListener('click', ['$event']) onClick(e?: Event) {
    if (e && (e.target as Element).className === 'options-item') {
      return;
    }

    if (!this.selectOpened) {
      this.filter.limit = this.limit;
      this.filter.offset = 0;
      this.allLoaded = false;
      this.selectOpened = true;

      this.loadOptions();
    }

    if (this.selectOpened
      && !this.elementRef.nativeElement.contains(event.target)) {
      this.closeSelect();
    } else {
      this.selectOpened = true;
    }
  }

  @HostListener('document:click', ['$event']) onClickOutside(event: MouseEvent): void {
    if (this.selectOpened
      && !this.elementRef.nativeElement.contains(event.target)) {
      this.closeSelect();
    }
  }

  @HostListener('keydown', ['$event'])
  keyboardInput(event: KeyboardEvent) {
    if (!this.selectOpened) {
      if (event.key === 'ArrowDown') {
        this.onClick();
      }

      return;
    }

    switch (event.key) {
      case 'Enter':
        // if (this.focusedItem) {
        // 	this.selectOption(this.focusedItem);
        // } else {
        this.closeSelect();
        // }
        break;
      case 'Escape':
        this.closeSelect();
        break;
    }
  }

  @ViewChild('modelInput', { static: false }) modelInput: ElementRef;
  @ViewChild('scrollDiv', { static: false }) scrollDiv: ElementRef;

  ngOnInit() {
    this.textFilterChanged.pipe(
      debounceTime(500),
      distinctUntilChanged())
      .subscribe(newFilter => {
        this.textFilter = newFilter;
        this.loadOptions();
      });
  }

  optionsChanged(items: any[]) {
    if (items.length > 0) {
      this.setValueFromInside(items[0]);
    }
    this.closeSelect();
  }

  onScroll() {
    // if (this.collection) {
    //   return;
    // }

    // scrollTop not always returns int
    const scrollDivElement = this.scrollDiv.nativeElement;
    const atBottom = scrollDivElement.scrollHeight - Math.ceil(scrollDivElement.scrollTop) === scrollDivElement.clientHeight;
    if (atBottom && !this.allLoaded) {
      this.loadOptions();
    }
  }

  // ControlValueAccessor implementation start
  propagateChange = (_: any) => { };
  propagateTouched = () => { };
  registerOnChange(fn: (_: any) => void) {
    this.propagateChange = fn;
  }
  registerOnTouched(fn: () => void) {
    this.propagateTouched = fn;
  }

  // set ngModel from outside
  writeValue(value: any) {
    this.selectedModel = value;
    this.changeDetectorRef.detectChanges();
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }
  // ControlValueAccessor implementation end

  selectOption(item: T) {
    this.setValueFromInside(item);
    this.closeSelect();
  }

  clearSelection(event: Event) {
    this.setValueFromInside(null);
    event.stopPropagation();
  }

  textFilterChange(textFilter: string) {
    this.filter.offset = 0;
    this.textFilterChanged.next(textFilter);
  }

  getTemplate(item: T) {
    return this.templatePipe.getResultText(item, this.textTemplate);
  }

  private loadOptions() {
    if (this.allLoaded) {
      return;
    }
    // if (!this.restUrl) {
    // 	this.options = this.collection;
    // 	if (this.filterCollectionBy && this.textFilter) {
    // 		this.options = this.options.filter(option => {
    // 			const innerProperties = this.filterCollectionBy.split('.');
    // 			let filteredProperty = innerProperties.length ? option : option[this.filterCollectionBy];
    // 			innerProperties.forEach(key => filteredProperty = filteredProperty[key]);
    // 			return filteredProperty.toString().toLowerCase().indexOf(this.textFilter!.toLowerCase()) !== -1;
    // 		});
    // 	}
    // 	this.loading = false;
    // } else {
    if (this.restUrl) {
      this.isLoading = true;
      this.getFiltered()
        .subscribe(e => {
          if (this.filter.offset) {
            if (e.length < this.limit) {
              this.allLoaded = true;
            }

            const tempArray = this.options.slice(0);
            tempArray.push(...e);
            this.options = tempArray;
          } else {
            this.options = e;
          }

          this.filter.offset = this.options.length;
          this.isLoading = false;
          this.changeDetectorRef.detectChanges();
        });
    }
  }

  private setValueFromInside(newValue: T | null) {
    this.selectedModel = newValue;
    this.propagateChange(newValue);
    this.propagateTouched();
  }

  private closeSelect() {
    this.selectOpened = false;
    this.textFilter = null;
    if (this.restUrl) {
      this.options = [];
      this.changeDetectorRef.detectChanges();
    }
  }

  private getQueryString(): string {
    let result = '';
    let isFirst = true;

    if (this.textFilter
      && this.textFilter.length > 0) {
      result = '?textFilter=' + this.textFilter;
      isFirst = false;
    }

    if (this.filter) {
      const keys = Object.keys(this.filter);
      keys.forEach(key => {
        if (this.filter[key] !== null && this.filter[key] !== undefined) {
          if (isFirst) {
            result = '?' + key + '=' + this.filter[key];
            isFirst = false;
          } else {
            result += '&' + key + '=' + this.filter[key];
          }
        }
      });
    }

    return result;
  }

  private getFiltered(): Observable<T[]> {
    return this.httpClient.get<any[]>(this.restUrl + this.getQueryString());
  }
}
