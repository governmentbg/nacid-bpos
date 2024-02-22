import { Component, Input } from "@angular/core";
import { SearchResultItem } from '../models/search-result-item';

@Component({
  selector: 'search-result',
  templateUrl: 'search-result.component.html',
  styleUrls: ['search-result.component.scss']
})
export class SearchResultComponent {
  @Input('model') model: SearchResultItem;
}
