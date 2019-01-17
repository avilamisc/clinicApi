import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PatientBookingRoutingModule } from './patient-booking-routing.module';
import { PatientBookingComponent } from 'src/app/components/patient/booking/patientBooking.component';
import { EditComponent } from 'src/app/components/patient/booking/edit/edit.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    PatientBookingComponent,
    EditComponent
  ],
  imports: [
    CommonModule,
    PatientBookingRoutingModule,
    FormsModule,
    ReactiveFormsModule
  ]
})
export class PatientBookingModule { }
