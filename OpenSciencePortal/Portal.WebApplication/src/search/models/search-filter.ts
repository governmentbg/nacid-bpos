import { SearchField, registerFieldTypeChangeHandler } from './search-field';
import { FormBuilder } from '@angular/forms';

export class SearchFilter {
  quickSearch: SearchField = new SearchField();
  fields: SearchField[] = [];
}

export function createFilterForm(fb: FormBuilder) {
  let quickSearch = fb.group(new SearchField());
  let firstField = fb.group(new SearchField());
  registerFieldTypeChangeHandler(firstField);

  return fb.group({
    quickSearch,
    fields: fb.array([firstField])
  });
}
