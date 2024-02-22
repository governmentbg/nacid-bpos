import { Component, OnInit } from '@angular/core';
import { SearchResource } from 'src/search/search.resource';
import { SearchFilterService } from 'src/search/search-filter.service';
import { SearchStateService } from 'src/search/search.state';
import { LoadingIndicatorService } from 'src/infrastructure/loading-indicator.service';
import { FormArray, FormGroup, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { createFilterForm } from 'src/search/models/search-filter';

@Component({
  templateUrl: './landing-page.component.html',
  styleUrls: ['./landing-page.component.scss']
})
export class LandingPageComponent {
  constructor(
    private router: Router
  ) { }

  onCategorySelect() {
    this.router.navigate(['/search'])
  }

  onSearch() {
    this.router.navigate(['/search']);
  }
}
