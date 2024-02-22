import { Component, OnInit } from '@angular/core';
import { SearchResource } from '../search.resource';
import { SearchFilter } from '../models/search-filter';
import { FormArray } from '@angular/forms';
import { SearchFilterService } from '../search-filter.service';
import { SearchStateService } from '../search.state';
import { LoadingIndicatorService } from 'src/infrastructure/loading-indicator.service';
import { PaginatorChangeEvent } from 'src/components/paginator/paginator.component';
import { SearchMode } from '../models/search-mode.enum';
import { SearchFieldCategories, SearchFieldType } from '../models/search-field-type.enum';
import { SearchField } from '../models/search-field';
import { SEARCH_FIELD_SPEC } from '../models/search-field-type-spec';

@Component({
  templateUrl: 'search.component.html',
  styleUrls: ['search.component.scss']
})

export class SearchComponent implements OnInit {
  constructor(
    private resource: SearchResource,
    private filterService: SearchFilterService,
    private state: SearchStateService,
    private loadingIndicator: LoadingIndicatorService
  ) { }

  get filterForm() {
    return this.state.filterForm;
  }

  get fields() {
    return this.state.filterForm.get('fields') as FormArray;
  }

  get data() {
    return this.state.data;
  }

  get metadata() {
    return this.state.metadata;
  }

  pageSizes = [10, 25, 50, 100];

  ngOnInit() {
    this.search();
  }

  search(searchMode: SearchMode = SearchMode.mixed) {
    this.metadata.filter = this.filterService.clone(this.filterForm.value as SearchFilter);
    this.metadata.searchMode = searchMode;

    console.log('Search mode is:', SearchMode[searchMode]);
    let queryFields = this.filterService.getQueryFields(this.filterForm.value, searchMode);
    console.log('Fields that will be included into the aggregation:', queryFields)
    let aggregationsQuery = this.filterService.buildAggregations(queryFields);
    let queryFilter = this.filterService.buildQueryFilter(queryFields, this.metadata.page, this.metadata.pageSize);

    this.loadingIndicator.start();
    this.resource.search(queryFilter, aggregationsQuery)
      .subscribe(res => {
        this.data.totalCount = res.totalCount;
        this.data.items = res.items;
        this.data.aggregations = res.aggregations;
        this.data.categories = this.formatCategories(res.categories);
        this.data.took = res.took;

        this.loadingIndicator.stop();
        this.metadata.showCategories = false;
        this.metadata.showSidebar = true;
      }, _ => this.loadingIndicator.stop());
  }

  handleFilterChange(shouldSearch: boolean) {
    if (shouldSearch)
      this.search();
  }

  handlePageEvent(event: PaginatorChangeEvent) {
    this.metadata.page = event.page;
    this.metadata.pageSize = event.pageSize;
    this.search(this.metadata.searchMode);
  }

  formatCategories(categories: any) {
    let fields = this.fields.value;
    Object.keys(categories).forEach((category: string) => {
      categories[category].forEach(item => {
        item.selected = fields.some((e: SearchField) => e.type == SearchFieldType[category]
          && e.searchTerm == item.key);
      });

      // This sorts the items so that selected are first
      // Uncomment if they want it, because in some cases selected items are not visible,
      // because the view is not collapsed
      //categories[category].sort((a, b) => b.selected - a.selected);
    });
    return categories;
  }
}
