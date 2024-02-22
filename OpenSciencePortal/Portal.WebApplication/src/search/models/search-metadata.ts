import { SearchFilter } from './search-filter';
import { SearchMode } from './search-mode.enum';

export class SearchMetadata {
  showAdvancedSearch: boolean;
  showCategories: boolean;
  showSidebar: boolean;
  filter: SearchFilter;
  shouldFilter: boolean;
  aggregationFilter: any;
  shouldAggregate: boolean;
  query: any;
  page: number;
  pageSize: number;
  searchMode: SearchMode;
}