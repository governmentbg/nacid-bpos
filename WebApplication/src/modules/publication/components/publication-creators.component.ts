import { AfterViewInit, ChangeDetectionStrategy, Component, EventEmitter, Output, ViewChild } from '@angular/core';
import { PublicationCreatorAffiliation } from '../models/publication-creator-affiliation.model';
import { PublicationCreatorIdentifier } from '../models/publication-creator-identifier.model';
import { PublicationCreator } from '../models/publication-creator.model';
import { MultiplePublicationEntityBaseComponent } from './multiple-publication-entity-base.component';

@Component({
  selector: 'publication-creators',
  templateUrl: 'publication-creators.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})

export class PublicationCreatorsComponent extends MultiplePublicationEntityBaseComponent<PublicationCreator> implements AfterViewInit {
  @ViewChild('creatorsForm', { static: false }) creatorsForm;

  @Output() isValidForm: EventEmitter<boolean> = new EventEmitter();

  constructor() {
    super(PublicationCreator);
  }

  ngAfterViewInit() {
    if (!this.disabled) {
      this.creatorsForm.statusChanges.subscribe(() => this.isValidForm.emit(this.creatorsForm.valid));
    }
  }

  addIdentifier(creator: PublicationCreator) {
    if (!creator.identifiers) {
      creator.identifiers = [];
    }

    const creatorIdentifier = new PublicationCreatorIdentifier();
    creatorIdentifier.viewOrder = creator.identifiers && creator.identifiers.length
      ? creator.identifiers[creator.identifiers.length - 1].viewOrder + 1
      : 1;
    creator.identifiers.push(creatorIdentifier);
  }

  removeIdentifier(creator: PublicationCreator, index: number) {
    creator.identifiers.splice(index, 1);
  }

  addAffiliation(creator: PublicationCreator) {
    if (!creator.affiliations) {
      creator.affiliations = [];
    }

    const creatorAffiliation = new PublicationCreatorAffiliation();
    creatorAffiliation.viewOrder = creator.affiliations && creator.affiliations.length
      ? creator.affiliations[creator.affiliations.length - 1].viewOrder + 1
      : 1;
    creator.affiliations.push(creatorAffiliation);
  }

  removeAffiliation(creator: PublicationCreator, index: number) {
    creator.affiliations.splice(index, 1);
  }
}
