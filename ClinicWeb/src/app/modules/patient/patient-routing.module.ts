import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PatientComponent } from 'src/app/components/patient/patient.component';

const routes: Routes = [
  {
    path: '',
    component: PatientComponent
  },
  {
    path: 'booking',
    loadChildren: './booking/patient-booking.module#PatientBookingModule'
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PatientRoutingModule { }
