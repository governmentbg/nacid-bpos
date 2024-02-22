import { SearchFieldMode } from './search-field-mode.enum';
import { SearchFieldType } from './search-field-type.enum';
import { SEARCH_FIELD_SPEC, SearchFieldControlType } from './search-field-type-spec';
import { FormGroup } from '@angular/forms';
import { clone } from '../../infrastructure/utils';

export class SearchField {
  searchTerm: string;
  name: string;
  mode: SearchFieldMode;
  type: SearchFieldType;
  controlType: SearchFieldControlType;
  controlMetadata: object;

  constructor(type?: SearchFieldType, value?: any, model?: any) {
    this.type = (type || SearchFieldType.all);
    this.name = SEARCH_FIELD_SPEC[this.type].name;
    this.mode = SEARCH_FIELD_SPEC[this.type].defaultMode;
    this.searchTerm = SEARCH_FIELD_SPEC[this.type].defaultValue;
    this.controlType = SEARCH_FIELD_SPEC[this.type].controlType;
    this.controlMetadata = clone(SEARCH_FIELD_SPEC[this.type].controlMetadata);

    if (value != undefined)
      this.searchTerm = value;

    if (model !== undefined)
      this.controlMetadata['model'] = model;
  }
}

function changeSearchFieldType(field: SearchField, type: SearchFieldType) {
  const cloned = clone(field);
  cloned.type = type;
  if (cloned.type) {
    cloned.name = SEARCH_FIELD_SPEC[cloned.type].name;
    cloned.mode = SEARCH_FIELD_SPEC[cloned.type].defaultMode;
    cloned.controlType = SEARCH_FIELD_SPEC[cloned.type].controlType;
    cloned.controlMetadata = SEARCH_FIELD_SPEC[cloned.type].controlMetadata;
    if (cloned.mode == SearchFieldMode.exact)
      cloned.searchTerm = SEARCH_FIELD_SPEC[cloned.type].defaultValue;
  }
  else {
    cloned.name = null;
    cloned.mode = null;
    cloned.searchTerm = null;
    cloned.controlType = null;
    cloned.controlMetadata = null;
  }
  return cloned;
}

export function registerFieldTypeChangeHandler(field: FormGroup) {
  field.get('type').valueChanges.subscribe(newValue => {
    field.reset(
      changeSearchFieldType(field.value, newValue),
      { emitEvent: false }
    );
  });
}

export function searchFieldIsEmpty(field: SearchField) {
  return field.searchTerm === SEARCH_FIELD_SPEC[field.type].defaultValue;
}