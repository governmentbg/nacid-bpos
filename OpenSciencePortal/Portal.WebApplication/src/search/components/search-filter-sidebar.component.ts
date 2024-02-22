import { Component, Input, Output, EventEmitter } from "@angular/core";
import { FormGroup, FormArray } from '@angular/forms';
import { SearchFieldCategories, SearchFieldType } from '../models/search-field-type.enum';
import { SEARCH_FIELD_SPEC, SearchFieldControlType } from '../models/search-field-type-spec';
import { SearchField } from '../models/search-field';
import { faTimes } from '@fortawesome/free-solid-svg-icons';
import { SearchStateService } from '../search.state';
import { Debug, DebugVerbosity } from 'src/infrastructure/debug.decorator';

@Component({
  selector: 'search-sidebar',
  templateUrl: 'search-filter-sidebar.component.html',
  styleUrls: ['search-filter-sidebar.component.scss']
})
export class SearchFilterSidebarComponent {
  constructor(
    private state: SearchStateService
  ) { }

  @Input()
  filterForm: FormGroup;

  get fields() {
    return this.filterForm.get('fields') as FormArray;
  }

  get data() {
    return this.state.data.categories;
  }

  @Output()
  search: EventEmitter<boolean> = new EventEmitter();

  SearchFieldType = SearchFieldType;
  SearchFieldControlType = SearchFieldControlType;
  SEARCH_FIELD_SPEC = SEARCH_FIELD_SPEC;
  categories = SearchFieldCategories;
  faTimesCircle = faTimes;

  // @Debug({ verbosity: DebugVerbosity.methodInvocation })
  handleCheckboxChange(fieldType: SearchFieldType, item: any) {
    if (item.selected)
      this.removeFilter(fieldType, item);
    else
      this.state.addField(new SearchField(fieldType, item.key, item.model));

    this.state.metadata.page = 0;
    this.search.emit(true);
  }

  handleFilterRemove(index: number) {
    if (this.removeAt(index))
      this.search.emit(true);
  }

  private removeFilter(fieldType: SearchFieldType, item: any) {
    let index = this.fields.value.findIndex((e: SearchField) => e.type == fieldType
      && e.searchTerm == item.key);

    this.removeAt(index);
  }

  // @Debug()
  private removeAt(index: number): boolean {
    if (index === 0 && this.fields.length === 1) {
      this.fields.get("0").setValue(new SearchField());
      return true;
    }
    else if (index > 0 && this.fields.length > 1) {
      this.fields.removeAt(index);
      return true;
    }
    else
      return false;
  }

  clearFilter() {
    this.state.resetFilter();
    this.state.metadata.page = 0;
    this.search.emit(true);
  }
}
