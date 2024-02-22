import { AfterViewInit, ChangeDetectionStrategy, Component, EventEmitter, Output, ViewChild } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { NameType } from '../enums/name-type.enum';
import { PublicationContributorIdentifier } from '../models/publication-contributor-identifier.model';
import { PublicationContributor } from '../models/publication-contributor.model';
import { MultiplePublicationEntityBaseComponent } from './multiple-publication-entity-base.component';

@Component({
  selector: 'publication-contributors',
  templateUrl: 'publication-contributors.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})

export class PublicationContributorsComponent extends MultiplePublicationEntityBaseComponent<PublicationContributor> implements AfterViewInit {
  @ViewChild('contributorsForm', { static: false }) contributorsForm;

  @Output() isValidForm: EventEmitter<boolean> = new EventEmitter();

  nameTypes = NameType;

  constructor(public translateService: TranslateService) {
    super(PublicationContributor);
  }

  ngAfterViewInit() {
    if (!this.disabled) {
      this.contributorsForm.statusChanges.subscribe(() => this.isValidForm.emit(this.contributorsForm.valid));
    }
  }

  contributorChanged(contributor: PublicationContributor) {
    const nameType = contributor.nameType;
    contributor = new PublicationContributor();
    contributor.nameType = nameType;
  }

  addIdentifier(contributor: PublicationContributor) {
    if (!contributor.identifiers) {
      contributor.identifiers = [];
    }

    const identifier = new PublicationContributorIdentifier();
    identifier.viewOrder = contributor.identifiers && contributor.identifiers.length
      ? contributor.identifiers[contributor.identifiers.length - 1].viewOrder + 1
      : 1;
    contributor.identifiers.push(identifier);
  }

  removeIdentifier(creator: PublicationContributor, index: number) {
    creator.identifiers.splice(index, 1);
  }
}
