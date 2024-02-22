import { Injectable } from "@angular/core";
import { FormGroup, FormBuilder, FormArray } from '@angular/forms';
import { SearchResultItem } from './models/search-result-item';
import { SearchMetadata } from './models/search-metadata';
import { createFilterForm } from './models/search-filter';
import { SearchMode } from './models/search-mode.enum';
import { SearchFieldType } from './models/search-field-type.enum';
import { SearchField, registerFieldTypeChangeHandler } from './models/search-field';

@Injectable()
export class SearchStateService {
  constructor(private fb: FormBuilder) {
    this.filterForm = createFilterForm(fb);

    this.data = {
      items: [],
      aggregations: null,
      categories: {},
      totalCount: 0,
      took: 0,
      timedOut: false
    };

    this.metadata = <SearchMetadata>{
      showAdvancedSearch: false,
      showCategories: true,
      showSidebar: false,
      filter: null,
      shouldFilter: null,
      aggregationFilter: null,
      shouldAggregate: true,
      searchMode: SearchMode.simple,
      query: null,
      page: 0,
      pageSize: 10
    };
  }

  filterForm: FormGroup;
  data: {
    items: Array<SearchResultItem>,
    aggregations: any,
    categories: any,
    totalCount: number,
    took: number;
    timedOut: boolean
  };
  metadata: SearchMetadata;

  get filterFields() {
    return this.filterForm.get('fields') as FormArray;
  }

  resetFilter() {
    this.filterForm = createFilterForm(this.fb);
    this.metadata.page = 0;
  }

  selectCategory(category: SearchFieldType, value: any) {
    let filterFields = this.filterForm.get('fields') as FormArray;
    let existingField = filterFields.value.find((e: SearchField) => e.type == category);

    if (existingField) {
      (<SearchField>existingField).searchTerm = value.key;
      (<SearchField>existingField).controlMetadata['model'] = value.model;
    }
    else {
      let field = new SearchField(category, value.key);
      field.controlMetadata['model'] = value.model;
      let fieldControl = this.fb.group(field);
      registerFieldTypeChangeHandler(fieldControl);
      filterFields.push(fieldControl);
    }

    this.metadata.page = 0;
    // This is not needed, push triggers change detection
    //filterFields.setValue(filterFields.value);
  }

  addField(field: SearchField) {
    let fieldControl = this.fb.group(field);
    registerFieldTypeChangeHandler(fieldControl);
    this.filterFields.push(fieldControl);
  }
}