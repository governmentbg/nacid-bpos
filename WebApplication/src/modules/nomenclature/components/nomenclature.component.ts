import { Component } from '@angular/core';

@Component({
  selector: 'app-nomenclatures',
  templateUrl: 'nomenclature.component.html'
})

export class NomenclatureComponent {
  navLinks: any[] = [
    {
      path: 'resourceTypes',
      label: 'Типове'
    },
    {
      path: 'generalResourceTypes',
      label: 'Типове на свързания ресурс'
    },
    {
      path: 'resourceIdentifierTypes',
      label: 'Идентификатори на ресурси'
    },
    {
      path: 'nameIdentifiers',
      label: 'Идентификатори на имена'
    },
    {
      path: 'organizationalIdentifiers',
      label: 'Идентификатори на организации'
    },
    {
      path: 'relationTypes',
      label: 'Типове релации'
    },
    {
      path: 'titleTypes',
      label: 'Типове заглавия'
    },
    {
      path: 'languages',
      label: 'Езици'
    },
    {
      path: 'contributorTypes',
      label: 'Типове участници'
    },
    {
      path: 'descriptionTypes',
      label: 'Типове описание'
    },
    {
      path: 'licenseTypes',
      label: 'Лицензи'
    },
    {
      path: 'accessRights',
      label: 'Права за достъп'
    },
    {
      path: 'audienceTypes',
      label: 'Типове публика'
    }
  ];
}
