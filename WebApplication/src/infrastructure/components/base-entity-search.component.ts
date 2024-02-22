import { OnInit } from '@angular/core';
import { BaseEntityFilterService } from '../base/interfaces/base-entity-filter.service';
import { IBaseEntityResource } from '../base/interfaces/base-entity-resource.interface';
import { Entity } from '../base/models/entity.model';
import { LoadingIndicatorService } from './loading-indicator/loading-indicator.service';

export abstract class BaseEntitySearchComponent<T extends Entity> implements OnInit {
  collection: any[] = [];

  abstract displayedColumns: string[];

  canLoadMore = false;

  constructor(
    public filter: BaseEntityFilterService,
    protected resource: IBaseEntityResource<T>,
    protected loadingIndicator: LoadingIndicatorService
  ) { }

  ngOnInit() {
    this.search();
  }

  search(loadMore?: boolean) {
    if (!loadMore) {
      this.filter.offset = 0;
    }

    this.loadingIndicator.show();
    this.resource.getFiltered(this.filter)
      .subscribe((collection: any[]) => {
        if (!this.filter.offset) {
          this.collection = collection;
        } else {
          this.collection = [...this.collection, ...collection];
        }

        this.canLoadMore = collection.length === this.filter.limit;
        this.filter.offset = this.collection.length;

        this.loadingIndicator.hide();
      });
  }

  clearFilter() {
    this.filter.clear();
    this.search();
  }

  loadMore() {
    this.filter.offset = this.collection.length;
    this.search(true);
  }
}
