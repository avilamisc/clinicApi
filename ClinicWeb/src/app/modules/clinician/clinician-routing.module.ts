import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ClinicianComponent } from 'src/app/components/clinician/clinician.component';

const routes: Routes = [
  {
    path: '',
    component: ClinicianComponent
  },
  {
    path: 'booking',
    loadChildren: './booking/clinician-booking.module#ClinicianBookingModule'
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ClinicianRoutingModule { }
