import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { PatientBookingComponent } from 'src/app/components/patient/booking/patientBooking.component';

const routes: Routes = [
  {
    path: '',
    component: PatientBookingComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PatientBookingRoutingModule { }
