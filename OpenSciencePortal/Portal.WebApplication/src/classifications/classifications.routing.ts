import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ScientificAreaComponent } from './components/scientific-area/scientific-area.component';
import { OrganizationComponent } from './components/organization/organization.component';
import { ScientificAreasDataResolverService } from './services/scientific-area-data-resolver.service';
import { OrganizationsDataResolverService } from './services/organization-data-resolver.service';
import { OrganizationViewComponent } from './components/organization-view/organization-view.component';
import { OrganizationViewDataResolverService } from './services/organization-view-data-resolver.service';

const routes: Routes = [
  {
    path: 'organizations',
    children: [
      {
        path: ':id',
        component: OrganizationViewComponent,
        resolve: {
          data: OrganizationViewDataResolverService
        }
      },
      {
        path: '',
        component: OrganizationComponent,
        resolve: {
          data: OrganizationsDataResolverService
        }
      }
    ]
  },
  {
    path: 'scientific-areas',
    component: ScientificAreaComponent,
    resolve: {
      data: ScientificAreasDataResolverService
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})

export class ClassificationsRouting { }