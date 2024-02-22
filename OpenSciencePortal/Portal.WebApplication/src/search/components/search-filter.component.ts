import { Component, Output, EventEmitter, Input, OnInit } from "@angular/core";
import { FormGroup, FormArray, FormBuilder } from '@angular/forms';
import { SearchFieldType, SearchFieldNumberValues } from '../models/search-field-type.enum';
import { SearchFieldControlType } from '../models/search-field-type-spec';
import { faMinus, faPlus } from '@fortawesome/free-solid-svg-icons';
import { SearchField, registerFieldTypeChangeHandler } from '../models/search-field';
import { BaseNomenclature } from 'src/components/async-select/models/base-nomenclature';
import { SearchMode } from '../models/search-mode.enum';
import { SearchStateService } from '../search.state';
import { createFilterForm } from '../models/search-filter';

@Component({
  selector: 'search-filter',
  templateUrl: 'search-filter.component.html',
  styleUrls: ['search-filter.component.scss']
})
export class SearchFilterComponent implements OnInit {
  constructor(
    private fb: FormBuilder,
    private state: SearchStateService
  ) { }

  get filterForm() {
    return this.state.filterForm;
  }

  get fields() {
    return this.state.filterForm.get('fields') as FormArray;
  }

  get showAdvancedSearch() {
    return this.state.metadata.showAdvancedSearch;
  }

  set showAdvancedSearch(value: boolean) {
    this.state.metadata.showAdvancedSearch = value;
  }

  @Input()
  reset: any;

  @Output()
  search: EventEmitter<SearchMode> = new EventEmitter<SearchMode>();

  SearchFieldTypeValues = SearchFieldNumberValues;
  SearchFieldType = SearchFieldType;
  SearchFieldControlType = SearchFieldControlType;
  SearchMode = SearchMode;
  faMinus = faMinus;
  faPlus = faPlus;

  ngOnInit() {
    if (this.reset !== undefined)
      this.state.filterForm = createFilterForm(this.fb);
  }

  addField() {
    this.state.addField(new SearchField());
  }

  removeField(index: number) {
    this.fields.removeAt(index);
  }

  handleNomenclatureModelChange(field: FormGroup, model: BaseNomenclature) {
    let controlMetadata = field.get('controlMetadata').value;
    controlMetadata.model = model;

    field.get('controlMetadata').setValue(controlMetadata);
  }

  onSearch(searchMode: SearchMode) {
    this.search.emit(searchMode);
  }
}