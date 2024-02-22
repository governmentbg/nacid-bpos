import { SearchResultItem } from './search-result-item';

export class SearchResult {
  items: SearchResultItem[];
  aggregations: any[];
  categories: any;
  totalCount: number;
  took: number;
  timedOut: boolean;
}
