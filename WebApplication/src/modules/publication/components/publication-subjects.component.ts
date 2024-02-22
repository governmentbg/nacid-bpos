import { AfterViewInit, ChangeDetectionStrategy, Component, EventEmitter, Output, ViewChild } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { PublicationSubject } from '../models/publication-subject.model';
import { MultiplePublicationEntityBaseComponent } from './multiple-publication-entity-base.component';

@Component({
  selector: 'publication-subjects',
  templateUrl: 'publication-subjects.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})

export class PublicationSubjectsComponent extends MultiplePublicationEntityBaseComponent<PublicationSubject> implements AfterViewInit {
  @ViewChild('subjectsForm', { static: false }) subjectsForm;

  @Output() isValidForm: EventEmitter<boolean> = new EventEmitter();

  constructor(public translateService: TranslateService) {
    super(PublicationSubject);
  }

  ngAfterViewInit() {
    if (!this.disabled) {
      this.subjectsForm.statusChanges.subscribe(() => this.isValidForm.emit(this.subjectsForm.valid));
    }
  }
}
