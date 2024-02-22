import { Component, OnInit, ViewChild, Input, ContentChild, TemplateRef, ElementRef, OnChanges, SimpleChanges, Output, EventEmitter } from "@angular/core";
import { ControlValueAccessor, FormControl, NG_VALUE_ACCESSOR } from '@angular/forms';
import { NgbDropdown } from '@ng-bootstrap/ng-bootstrap';
import { of, merge } from 'rxjs';
import { debounceTime, distinctUntilChanged, tap, switchMap } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { Configuration } from 'src/infrastructure/configuration/configuration';
import { faAngleDown, faSpinner, faSearch } from '@fortawesome/free-solid-svg-icons';
import { parseScrollEvent, ScrollType } from 'src/infrastructure/utils';
import { BaseNomenclature } from './models/base-nomenclature';
import { BaseFilter } from './models/base-filter';


@Component({
  selector: 'ab-async-select',
  templateUrl: 'async-select.component.html',
  styleUrls: ['async-select.component.scss', '../select/select.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: AsyncSelectComponent,
      multi: true
    }
  ]
})
export class AsyncSelectComponent<T extends BaseNomenclature> implements ControlValueAccessor, OnInit, OnChanges {
  constructor(private http: HttpClient, private config: Configuration) { }

  @Input()
  restUrl: string;

  @Input()
  placeholder: string;

  @Output()
  modelChange: EventEmitter<T> = new EventEmitter<T>();

  @ContentChild('dropdownItemTemplate', { static: true })
  dropdownItemTemplate: TemplateRef<ElementRef>;

  @ContentChild('selectedItemTemplate', { static: true })
  selectedItemTemplate: TemplateRef<ElementRef>;

  @ViewChild('dropdown', { static: true })
  dropdown: NgbDropdown;

  @ViewChild('dropdownMenu', { static: true })
  dropdownMenu: ElementRef;

  searchControl = new FormControl("");
  loadMore$ = new EventEmitter<boolean>();

  search$ = merge(
    this.searchControl.valueChanges.pipe(
      distinctUntilChanged()
    ),
    this.loadMore$,
  ).pipe(
    debounceTime(300),
    switchMap(value => {
      if (typeof value === 'boolean')
        return of({
          textFilter: this.searchControl.value,
          limit: this.limit,
          offset: this.offset,
          replaceExisting: false
        });
      else
        return of({
          textFilter: value,
          limit: this.limit,
          offset: 0,
          replaceExisting: true
        });
    })
  );

  id: number;
  nomenclature: T;
  isOpen: boolean;
  collection: T[] = [];
  loading = false;
  initialLoad = false;
  formControlChangeHandler: any;

  faAngleDown = faAngleDown;
  faSpinner = faSpinner;
  faSearch = faSearch;

  limit = 25;
  offset = 0;

  ngOnInit(): void {
    this.dropdown.openChange
      .subscribe((isOpen: boolean) => {
        this.isOpen = isOpen;
        this.searchControl.setValue('');
        if (this.isOpen && this.initialLoad) {
          this.getFiltered();
          this.initialLoad = true;
        }
      });

    this.search$.subscribe(filter => this.getFiltered(filter));
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['restUrl'] && changes['restUrl'].previousValue)
      this.getFiltered();
  }

  registerOnChange(fn: any): void {
    this.formControlChangeHandler = fn;
  }

  registerOnTouched(fn: any): void { }

  writeValue(id: number): void {
    this.id = id;
    this.loading = true;
    this.getNomenclature(id)
      .subscribe(nomenclature => {
        this.nomenclature = nomenclature;
        this.modelChange.emit(this.nomenclature);
        this.loading = false;
      }, _ => this.loading = false);
  }

  getFiltered(filter?: BaseFilter) {
    if (!filter)
      filter = new BaseFilter();

    this.loading = true;
    this.http
      .get<T[]>(`${this.config.restUrl}/${this.restUrl}?textFilter=${filter.textFilter}&limit=${filter.limit}&offset=${filter.offset}`)
      .subscribe(items => {
        if (filter.replaceExisting)
          this.collection = items;
        else
          this.collection.push(...items);
        this.loading = false;
      }, _ => this.loading = false);
  }

  getNomenclature(id: number) {
    if (!id)
      return of(null);

    let item = this.collection.find(e => e.id == id);

    if (item)
      return of(item);
    else
      return this.http.get<T>(`${this.config.restUrl}/${this.restUrl}/${id}`);
  }

  selectItem(item: T) {
    this.id = item.id;
    this.nomenclature = item;
    this.modelChange.emit(this.nomenclature);

    if (this.formControlChangeHandler)
      this.formControlChangeHandler(this.id);
  }

  handleScroll(event: WheelEvent) {
    var eventType = parseScrollEvent(event, this.dropdownMenu.nativeElement);

    if (eventType == ScrollType.bottom) {
      this.offset = this.collection.length;
      this.loadMore$.emit(true);
    }

    if (eventType != ScrollType.middle)
      event.preventDefault();
  }
}