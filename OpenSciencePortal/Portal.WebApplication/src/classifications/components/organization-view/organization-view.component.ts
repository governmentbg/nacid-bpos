import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { faSearch } from '@fortawesome/free-solid-svg-icons';
import { SearchStateService } from 'src/search/search.state';
import { SearchField } from 'src/search/models/search-field';
import { SearchFieldType } from 'src/search/models/search-field-type.enum';
import { Classification } from 'src/classifications/models/classification';

@Component({
  selector: 'app-organization-view',
  templateUrl: './organization-view.component.html',
  styleUrls: [
    '../../styles/common.scss',
    './organization-view.component.scss'
  ]
})
export class OrganizationViewComponent {
  constructor(
    route: ActivatedRoute,
    private state: SearchStateService,
    private router: Router
  ) {
    route.data.subscribe(({ data }) => this.organization = data);
  }

  organization: Classification;
  faSearch = faSearch;

  clickClassification(classification: Classification) {
    // If we are not in a leaf node
    if (classification.children && classification.children.length > 0) {
      classification.isOpen = !classification.isOpen;
    }
    // If we are in a leaf node
    else {
      this.selectClassification(classification);
    }
  }

  selectClassification(classification: Classification) {
    this.state.resetFilter();
    this.state.addField(new SearchField(SearchFieldType.classifications, classification.id, classification));
    this.router.navigate(['/search']);
  }
}
