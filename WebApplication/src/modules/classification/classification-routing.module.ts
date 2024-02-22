import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from 'src/infrastructure/interceptors/auth.guard';
import { ClassificationsComponent } from './components/classifications.component';

const routes: Routes = [
  {
    path: 'classifications',
    children: [
      {
        path: '',
        component: ClassificationsComponent,
        canActivate: [AuthGuard]
      }
    ]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})

export class ClassificationRoutingModule { }
