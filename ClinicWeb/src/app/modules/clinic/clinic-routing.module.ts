import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ClinicComponent } from 'src/app/components/clinic/clinic.component';

const routes: Routes = [
  {
    path: '',
    component: ClinicComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ClinicRoutingModule { }
