import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from 'src/infrastructure/interceptors/auth.guard';
import { InstitutionEditComponent } from './components/institution-edit.component';
import { InstitutionSearchComponent } from './components/institution-search.component';

const routes: Routes = [
  {
    path: 'institutions',
    children: [
      {
        path: '',
        component: InstitutionSearchComponent,
        canActivate: [AuthGuard]
      },
      {
        path: ':id',
        component: InstitutionEditComponent,
        canActivate: [AuthGuard]
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class InstitutionRoutingModule { }
