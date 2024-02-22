import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SearchStateService } from 'src/search/search.state';
import { SearchField } from 'src/search/models/search-field';
import { SearchFieldType } from 'src/search/models/search-field-type.enum';
import { Classification } from 'src/classifications/models/classification';

@Component({
  templateUrl: './organization.component.html',
  styleUrls: [
    '../../styles/common.scss',
    './organization.component.scss'
  ]
})
export class OrganizationComponent {
  constructor(
    route: ActivatedRoute,
    private state: SearchStateService,
    private router: Router
  ) {
    route.data.subscribe(({ data }) => {
      this.data = data;
    });
  }

  SearchFieldType = SearchFieldType;
  data: Classification[] = [];

  selectArea(type: SearchFieldType, area: Classification) {
    this.state.resetFilter();
    this.state.addField(new SearchField(type, area.id, area));
    this.router.navigate(['/search']);
  }
}
