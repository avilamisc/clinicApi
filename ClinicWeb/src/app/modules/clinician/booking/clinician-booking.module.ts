import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ClinicianBookingRoutingModule } from './clinician-booking-routing.module';
import { ClinicianBookingComponent } from 'src/app/components/clinician/booking/clinicianBooking.component';

@NgModule({
  declarations: [
    ClinicianBookingComponent
  ],
  imports: [
    CommonModule,
    ClinicianBookingRoutingModule
  ]
})
export class ClinicianBookingModule { }
