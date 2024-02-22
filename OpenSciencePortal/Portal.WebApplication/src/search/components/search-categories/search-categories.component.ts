import { Component, OnInit } from '@angular/core';
import { SearchFieldType, SearchFieldCategories } from 'src/search/models/search-field-type.enum';
import { SearchResource } from 'src/search/search.resource';
import { SearchStateService } from 'src/search/search.state';
import { SearchFilterService } from 'src/search/search-filter.service';
import { SEARCH_FIELD_SPEC } from '../../models/search-field-type-spec';
import { Router } from '@angular/router';
import { LoadingIndicatorComponent } from 'src/components/loading-indicator/loading-indicator.component';
import { LoadingIndicatorService } from 'src/infrastructure/loading-indicator.service';

@Component({
  selector: 'app-search-categories',
  templateUrl: './search-categories.component.html',
  styleUrls: ['./search-categories.component.scss']
})
export class SearchCategoriesComponent implements OnInit {

  constructor(
    private resource: SearchResource,
    private queryBuilder: SearchFilterService,
    private state: SearchStateService,
    private router: Router,
    private loadingIndicator: LoadingIndicatorService
  ) { }

  categoryData: any = {};
  SEARCH_FIELD_SPEC = SEARCH_FIELD_SPEC;
  SearchFieldType = SearchFieldType;
  categories = SearchFieldCategories;

  ngOnInit() {
    this.getCategories();
  }

  get filter() {
    return this.state.filterForm.value;
  }

  getCategories() {
    let aggregationsQuery = this.queryBuilder.buildAggregations([]);
    this.loadingIndicator.start();
    this.resource.search(null, aggregationsQuery).subscribe(res => {
      this.categoryData = res.categories;
      this.loadingIndicator.stop();
    });
  }

  selectCategory(category: SearchFieldType, value: any) {
    this.state.selectCategory(category, value);
    this.router.navigate(['/search']);
  }
}
