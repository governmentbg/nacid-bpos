import { Injectable } from "@angular/core";
import { SearchFilter } from './models/search-filter';
import { SearchField } from './models/search-field';
import { SearchFieldTypeValuesArray, SearchFieldType, SearchFieldNumberValues, SearchFieldCategories } from './models/search-field-type.enum';
import { SEARCH_FIELD_SPEC, SearchFieldTypeSpec } from './models/search-field-type-spec';
import { SearchFieldMode } from './models/search-field-mode.enum';
import { FormBuilder, FormGroup } from '@angular/forms';
import { SearchMetadata } from './models/search-metadata';
import { SearchMode } from './models/search-mode.enum';

@Injectable()
export class SearchFilterService {
  clone(filter: SearchFilter) {
    return JSON.parse(JSON.stringify(filter));
  }

  private getFieldQuery(field: SearchField) {
    if (field.mode === SearchFieldMode.allFts) {
      return {
        multi_match: {
          query: field.searchTerm,
          fields: SearchFieldTypeValuesArray
            .filter(field => SEARCH_FIELD_SPEC[SearchFieldType[field]].defaultMode == SearchFieldMode.fts)
            .map(field => SEARCH_FIELD_SPEC[SearchFieldType[field]].name)
        }
      };
    }
    else if (field.mode === SearchFieldMode.fts) {
      return {
        match: {
          [field.name]: field.searchTerm
        }
      };
    }
    else {
      return {
        term: {
          [field.name]: field.searchTerm
        }
      };
    }
  }

  private buildFilterForFieldType(items: SearchField[]) {
    if (items.length === 0)
      return null;
    else if (items.length === 1) {
      return this.getFieldQuery(items[0]);
    }
    else
      return {
        bool: {
          should: items.map(this.getFieldQuery)
        }
      };
  }

  getQueryFields(filter: SearchFilter, searchMode: SearchMode) {
    let result = [];
    if ((searchMode == SearchMode.simple || searchMode == SearchMode.mixed) && filter.quickSearch.searchTerm !== SEARCH_FIELD_SPEC[SearchFieldType.all].defaultValue)
      result.push(filter.quickSearch);

    if (searchMode == SearchMode.advanced || searchMode == SearchMode.mixed)
      result.push(...filter.fields.filter(field => field.searchTerm != SEARCH_FIELD_SPEC[field.type].defaultValue));

    return result;
  }

  buildAggregations(fields: SearchField[]) {
    let result = {
      aggs: {},
      size: 0
    };

    SearchFieldCategories.forEach(category => {
      //Get all filters except the filter for the current category
      let categoryFilter = SearchFieldNumberValues
        .filter(fieldType => fieldType != category)
        .map(fieldType => {
          let filteredFields = fields.filter(field => field.type == fieldType && field.searchTerm != SEARCH_FIELD_SPEC[field.type].defaultValue);
          return this.buildFilterForFieldType(filteredFields);
        })
        .filter(e => e);

      let spec = SEARCH_FIELD_SPEC[category];

      result.aggs[spec.name] = {
        filter: {
          bool: {
            filter: categoryFilter
          }
        },
        aggs: {
          filteredAggregation: {
            terms: {
              field: spec.name,
              size: 1000000
            }
          }
        }
      };
    });

    return result;
  }

  buildQueryFilter(searchFields: SearchField[], page: number = 0, pageSize: number = 10) {
    let ftsFields = searchFields.filter(field => field.mode == SearchFieldMode.fts && field.searchTerm);
    let filterFields = searchFields.filter(field => field.mode == SearchFieldMode.exact && field.searchTerm);
    let allFtsFields = searchFields.filter(field => field.mode == SearchFieldMode.allFts && field.searchTerm);

    let mustQuery = [];
    let filterQuery = [];

    let allFtsCategoryQuery = this.buildFilterForFieldType(allFtsFields);
    if (allFtsCategoryQuery)
      mustQuery.push(allFtsCategoryQuery);

    for (let category of SearchFieldNumberValues) {
      let ftsItems = ftsFields.filter(field => field.type == category);
      let filterItems = filterFields.filter(field => field.type == category);

      let ftsCategoryQuery = this.buildFilterForFieldType(ftsItems);
      let filterCategoryQuery = this.buildFilterForFieldType(filterItems);

      if (ftsCategoryQuery)
        mustQuery.push(ftsCategoryQuery);

      if (filterCategoryQuery)
        filterQuery.push(filterCategoryQuery);
    }

    return {
      query: {
        bool: {
          must: mustQuery,
          filter: filterQuery
        }
      },
      from: pageSize * page,
      size: pageSize
    };
  }
}

// // HOW to make highlights, putting it here just in case we ever need it
// let highlight = ftsFields
// .reduce((acc: object, curr: SearchField) => { acc[curr.name] = {}; return acc; }, {});

// if (allFtsCategoryQuery)
// highlight = this.getAllFtsFields()
//   .reduce((acc, field) => { acc[field] = {}; return acc; }, {});

// let highlightQuery = {
// pre_tags: [`<b>`],
// post_tags: ["</b>"],
// order: "score",
// fields: highlight
// };