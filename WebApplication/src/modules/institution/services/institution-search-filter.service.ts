import { Injectable } from '@angular/core';
import { BaseEntityFilterService } from 'src/infrastructure/base/interfaces/base-entity-filter.service';

@Injectable()
export class InstitutionSearchFilterService extends BaseEntityFilterService {
  name: string;
  repositoryUrl: string;

  searchInForeignNameOnly: boolean | null;

  constructor() {
    super(10);
  }

  clear() {
    this.limit = 10;
    this.offset = 0;

    this.name = null;
    this.repositoryUrl = null;
    this.searchInForeignNameOnly = null;
  }
}
