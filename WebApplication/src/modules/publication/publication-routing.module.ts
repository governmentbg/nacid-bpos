import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from 'src/infrastructure/interceptors/auth.guard';
import { PublicationSearchComponent } from './components/publication-search.component';
import { PublicationComponent } from './components/publication.component';

const routes: Routes = [
  {
    path: 'publications',
    children: [
      {
        path: '',
        component: PublicationSearchComponent,
        canActivate: [AuthGuard]
      },
      {
        path: ':id/edit',
        component: PublicationComponent,
        canActivate: [AuthGuard]
      },
      {
        path: 'new',
        component: PublicationComponent,
        canActivate: [AuthGuard]
      }
    ]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})

export class PublicationRoutingModule { }
