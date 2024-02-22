import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PublicationViewComponent } from './components/publication-view/publication-view.component';
import { PublicationViewResolverService } from './services/publication-view-resolver.service';
import { PublicationDetailComponent } from './components/publication-detail/publication-detail.component';
import { PublicationDetailResolverService } from './services/publication-detail-resolver.service';

const routes: Routes = [
  {
    path: 'publication',
    children: [
      {
        path: ':id',
        children: [
          {
            path: 'details',
            component: PublicationDetailComponent,
            resolve: {
              publication: PublicationDetailResolverService
            }
          },
          {
            path: '',
            component: PublicationViewComponent,
            resolve: {
              publication: PublicationViewResolverService
            },
          }
        ]
      }
    ]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})

export class PublicationRouting { }