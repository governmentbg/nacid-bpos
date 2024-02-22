import { Injectable } from '@angular/core';
import { BaseEntityFilterService } from 'src/infrastructure/base/interfaces/base-entity-filter.service';
import { Institution } from 'src/modules/institution/models/institution.model';
import { PublicationStatus } from '../enums/publication-status.enum';

@Injectable()
export class PublicationSearchFilterService extends BaseEntityFilterService {
  constructor() {
    super(10);
  }

  title: string;
  creatorFirstName: string;
  creatorLastName: string;

  status: PublicationStatus | null;

  moderatorInstitutionId: number | null;
  moderatorInstitution: Institution;

  clear() {
    this.limit = 10;
    this.offset = 0;

    this.title = null;
    this.creatorFirstName = null;
    this.creatorLastName = null;
    this.status = null;
    this.moderatorInstitutionId = null;
    this.moderatorInstitution = null;
  }
}
