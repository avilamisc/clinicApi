import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ClinicianBookingComponent } from 'src/app/components/clinician/booking/clinicianBooking.component';

const routes: Routes = [
  {
    path: '',
    component: ClinicianBookingComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ClinicianBookingRoutingModule { }
