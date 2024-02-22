import { Component } from '@angular/core';
import { BaseEntitySearchComponent } from 'src/infrastructure/components/base-entity-search.component';
import { LoadingIndicatorService } from 'src/infrastructure/components/loading-indicator/loading-indicator.service';
import { User } from '../models/user.model';
import { UserResource } from '../resources/user.resource';
import { UserSearchFilterService } from '../services/user-search-filter.service';

@Component({
  selector: 'user-search',
  templateUrl: 'user-search.component.html'
})
export class UserSearchComponent extends BaseEntitySearchComponent<User> {
  displayedColumns: string[] = ['actions', 'username', 'fullname', 'role', 'createDate', 'updateDate', 'isActive', 'isLocked'];

  constructor(
    public filter: UserSearchFilterService,
    protected resource: UserResource,
    protected loadingIndicator: LoadingIndicatorService
  ) {
    super(filter, resource, loadingIndicator);
  }
}
