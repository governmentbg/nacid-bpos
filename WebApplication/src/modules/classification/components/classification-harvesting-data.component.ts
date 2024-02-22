import { Component, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MatExpansionPanel } from '@angular/material/expansion';
import { TranslateService } from '@ngx-translate/core';
import { MetadataHarvestingService } from 'src/infrastructure/services/metadata-harvesting.service';
import { Classification } from 'src/modules/classification/models/classification.model';
import { LoadingIndicatorService } from './../../../infrastructure/components/loading-indicator/loading-indicator.service';

@Component({
  selector: 'classification-harvesting-data',
  templateUrl: 'classification-harvesting-data.component.html'
})

export class ClassificationHarvestingDataComponent {
  @Input() isNew: boolean;

  @Output() selectedSetsChange: EventEmitter<string[]> = new EventEmitter<string[]>();

  @Output() isValid: EventEmitter<boolean> = new EventEmitter();

  classification: Classification;
  @Input('classification')
  set model(classification: Classification) {
    this.classification = classification;

    if (!classification) {
      return;
    }

    if (classification.metadataFormat) {
      this.selectedMetadataFormat = { name: classification.metadataFormat };
    }

    if (this.classification.isReadonly && this.classification.harvestUrl) {
      this.validateRepositoryUrl(this.classification.harvestUrl);
    }
  }

  isValidRepositoryUrl: boolean;
  repositorySetsCache: Map<string, { spec: string, name: string, selected: boolean }[]> = new Map<string, { spec: string, name: string, selected: boolean }[]>();

  repositorySets: { spec: string, name: string, selected: boolean }[] = [];
  selectedMetadataFormat: { name: string };
  classificationSets: Set<string> = new Set<string>();

  @ViewChild('harvestingDataForm', { static: false }) harvestingDataForm: NgForm;
  @ViewChild(MatExpansionPanel, { static: false }) panel: MatExpansionPanel;

  constructor(
    private metadataHarvestingService: MetadataHarvestingService,
    private loadingIndicator: LoadingIndicatorService,
    public translateService: TranslateService
  ) {
  }

  ngAfterViewInit() {
    this.harvestingDataForm.statusChanges.subscribe(() => this.isValid.emit(this.harvestingDataForm.valid));
  }

  selectedMetadataFormatChanged(value: string | null) {
    if (!value) {
      this.selectedMetadataFormat = null;
    } else {
      this.selectedMetadataFormat = { name: value };
    }

    this.classification.metadataFormat = value;
  }

  selectedSetChange() {
    const selectedSets = this.repositorySets
      .filter(s => s.selected)
      .map(s => s.spec);

    this.selectedSetsChange.emit(selectedSets);
  }

  clearMetadataFormat() {
    this.classification.metadataFormat = null;
    this.selectedMetadataFormat = null;
  }

  validateRepositoryUrl(url: string) {
    if (this.panel) {
      this.panel.close();
    }

    if (this.repositorySetsCache.has(url)) {
      this.isValidRepositoryUrl = true;
      this.harvestingDataForm.controls['classificationHarvestUrl'].setErrors(null);
      return;
    }

    this.repositorySets = [];

    if (!url) {
      this.harvestingDataForm.controls['classificationHarvestUrl'].setErrors({ 'invalidUrl': true });
      return;
    }

    this.loadingIndicator.show();

    return this.metadataHarvestingService.validateRepositoryUrl(url)
      .subscribe(isValid => {
        this.isValidRepositoryUrl = isValid;
        if (!isValid) {
          this.harvestingDataForm.controls['classificationHarvestUrl'].setErrors({ 'invalidUrl': true });
        } else {
          this.harvestingDataForm.controls['classificationHarvestUrl'].setErrors(null);
        }

        this.loadingIndicator.hide();
      });
  }

  setClassificationSets() {
    const selectedSets = this.repositorySets
      .filter(s => s.selected)
      .map(s => s.spec);

    if (selectedSets.length !== this.repositorySets.length) {
      this.classification.sets = selectedSets;
    }
  }

  loadRepositorySets(repositoryUrl: string) {
    if (this.repositorySetsCache.has(repositoryUrl)) {
      this.repositorySets = this.repositorySetsCache.get(repositoryUrl);
      return;
    }

    this.loadingIndicator.show();

    return this.metadataHarvestingService.getRepositorySets(repositoryUrl)
      .subscribe(sets => {
        this.setRepositorySets(sets);
        this.repositorySetsCache.set(repositoryUrl, this.repositorySets);
        this.loadingIndicator.hide();
      });
  }

  changeSelectAll(select: boolean) {
    this.repositorySets.forEach(set => set.selected = select);
  }

  private setRepositorySets(repositorySets: { name: string, spec: string }[]) {
    if (!this.isNew) {
      this.setClassificationSelectedSets(this.classification.sets);
    }

    this.repositorySets = repositorySets.map(set => {
      return {
        name: set.name,
        spec: set.spec,
        selected: this.isNew || this.classificationSets.has(set.spec)
      }
    });
  }

  private setClassificationSelectedSets(sets: string[]) {
    (sets || []).forEach(set => this.classificationSets.add(set));
  }
}
