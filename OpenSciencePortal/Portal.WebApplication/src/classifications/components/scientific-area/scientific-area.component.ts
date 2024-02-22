import { Component, OnInit } from '@angular/core';
import { ActivatedRouteSnapshot, ActivatedRoute, Router } from '@angular/router';
import { SearchStateService } from 'src/search/search.state';
import { SearchFieldType } from 'src/search/models/search-field-type.enum';
import { SearchField } from 'src/search/models/search-field';
import { Classification } from 'src/classifications/models/classification';

@Component({
  templateUrl: './scientific-area.component.html',
  styleUrls: [
    '../../styles/common.scss',
    './scientific-area.component.scss'
  ]
})
export class ScientificAreaComponent {
  constructor(
    route: ActivatedRoute,
    private state: SearchStateService,
    private router: Router
  ) {
    route.data.subscribe(({ data }) => {
      this.data = data;
    });
  }

  data: Classification[] = [];

  selectArea(area: Classification) {
    this.state.resetFilter();
    this.state.addField(new SearchField(SearchFieldType.classifications, area.id, area));
    this.router.navigate(['/search']);
  }
}
